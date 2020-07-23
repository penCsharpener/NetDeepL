using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NetDeepL.Abstractions;
using NetDeepL.Models;
using NetDeepL.TranslationWorker.Abstractions;
using NetDeepL.TranslationWorker.Models;
using NetDeepL.TranslationWorker.Models.Config;
using Polly;

namespace NetDeepL.TranslationWorker.Implementations
{
    public class WorkbookTranslator : IWorkbookTranslator
    {
        private const string DEEPL_PLACEHOLDER = "_DeepL_";
        private readonly IAppInformation _appInformation;
        private INetDeepL _deepL;
        private readonly IOptions<ConfigFile> _options;
        private readonly IServiceProvider _serviceProvider;

        public WorkbookTranslator(IAppInformation appInformation, INetDeepL deepL, IOptions<ConfigFile> options, IServiceProvider serviceProvider)
        {
            _appInformation = appInformation;
            _deepL = deepL;
            _options = options;
            _serviceProvider = serviceProvider;
        }

        public async Task TranslateAsync()
        {
            var usageBefore = await _deepL.GetUsage();
            Console.WriteLine();
            Console.WriteLine("Translation starting");
            Console.WriteLine($"Api usage: {usageBefore.CharacterCount}/{usageBefore.CharacterLimit}");
            Console.WriteLine();

            foreach (var xlsx in _appInformation.GetExcelFiles(_options.Value.InputPath))
            {
                Console.WriteLine();
                Console.WriteLine($"processing file '{xlsx.Name}'");

                using (var wb = new XLWorkbook(xlsx.FullName))
                {
                    if (!_options.Value.OutputPath.Exists)
                    {
                        Directory.CreateDirectory(_options.Value.OutputPath.FullName);
                    }

                    var xlsxCopy = Path.Combine(_options.Value.OutputPath.FullName, xlsx.Name.Insert(xlsx.Name.Length - 5, "_NetDeepL"));
                    File.Copy(xlsx.FullName, xlsxCopy, true);

                    using (var wbCopy = new XLWorkbook(xlsxCopy))
                    {
                        try
                        {
                            CheckForAlreadyTranslatedWorksheets(wbCopy);

                            await ProcessSheets(wbCopy, _options.Value.LanguagesToTranslate, _options.Value.DelayMilliseconds);
                            Console.WriteLine();

                            // delete original sheets in copied workbook
                            for (int i = wbCopy.Worksheets.Where(x => x.Name.IndexOf(DEEPL_PLACEHOLDER) == -1).ToList().Count - 1; i >= 0; i--)
                            {
                                wbCopy.Worksheets.Where(x => x.Name.IndexOf(DEEPL_PLACEHOLDER) == -1).ToList()[i].Delete();
                            }

                            wbCopy.Save();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }
            }

            Console.WriteLine();
            var usageAfter = await _deepL.GetUsage();
            Console.WriteLine("Translation finished");
            Console.WriteLine($"Api usage: {usageAfter.CharacterCount}/{usageAfter.CharacterLimit}");
            Console.WriteLine();
        }

        private async Task ProcessSheets(IXLWorkbook workbook, Languages[] languagesToTranslate, int delay)
        {
            foreach (IXLWorksheet ws in workbook.Worksheets.Where(x => x.Name.IndexOf(DEEPL_PLACEHOLDER) == -1).ToList())
            {
                Console.WriteLine($"Beginning worksheet '{ws.Name}'");

                foreach (var targetLang in languagesToTranslate)
                {
                    Console.WriteLine($"Language '{targetLang}'");

                    var usedCells = ws.CellsUsed().Cast<IXLCell>()
                                                  .Where(x => x.Value != null)
                                                  .Select(x => new ExcelCell(x))
                                                  .ToList();

                    // don't create new sheet for sheets that are empty
                    if (usedCells.Count > 0)
                    {
                        var translatedSheet = workbook.Worksheets.Add($"{ws.Name}{DEEPL_PLACEHOLDER}{targetLang}");

                        foreach (var cell in usedCells)
                        {
                            await TranslateCell(cell, translatedSheet, delay, targetLang);
                        }

                        Console.WriteLine();
                    }
                }
                Console.WriteLine();
            }
        }

        private async Task TranslateCell(ExcelCell cell, IXLWorksheet translatedSheet, int delay, Languages targetLanguage)
        {
            translatedSheet.Cell(cell.Address).Style = cell.WrappedCell.Style;
            translatedSheet.Cell(cell.Address).FormulaA1 = cell.WrappedCell.FormulaA1;
            translatedSheet.Cell(cell.Address).FormulaR1C1 = cell.WrappedCell.FormulaR1C1;
            translatedSheet.Cell(cell.Address).FormulaReference = cell.WrappedCell.FormulaReference;

            var translation = cell.Text;
            if (!cell.WrappedCell.HasFormula)
            {
                if (!cell.IsHidden)
                {
                    // prevent status code 429 by sending request too quickly
                    await Task.Delay(delay);
                    try
                    {
                        translation = await Policy.HandleInner<SocketException>()
                            .RetryAsync(3, async (ex, retryCount) =>
                            {
                                await Task.Delay(delay);
                                _deepL = _serviceProvider.GetService<INetDeepL>();
                            })
                            .ExecuteAsync(async () =>
                            {
                                var response = await _deepL.TranslateAsync(cell.Text, targetLanguage, _options.Value.SourceLanguage);
                                return response.Text;
                            });
                    }
                    catch (SocketException socketEx)
                    {
                        Console.WriteLine(socketEx.ToString());
                    }
                    catch (IOException ioEx)
                    {
                        Console.WriteLine(ioEx.ToString());
                    }
                    catch (TaskCanceledException taskEx)
                    {
                        Console.WriteLine(taskEx.ToString());
                    }

                    Console.WriteLine($"{cell.Address} \"{cell.Text}\" => \"{translation}\"");
                }

                translatedSheet.Cell(cell.Address).Value = translation;
            }
        }

        private void CheckForAlreadyTranslatedWorksheets(IXLWorkbook wb)
        {
            var alreadyTranslated = wb.Worksheets.Where(x => x.Name.IndexOf(DEEPL_PLACEHOLDER) >= 0).ToList();

            if (alreadyTranslated.Count > 0)
            {
                Console.WriteLine("This file was already translated. " +
                    "Delete all the sheets with _DeepL_ in it to retranslate it.");
                Console.WriteLine("Do you want to delete translated sheet now? y/n");
                if (Console.ReadLine() == "y")
                {
                    for (int i = alreadyTranslated.Count - 1; i >= 0; i--)
                    {
                        Console.WriteLine($"Deleting sheet '{alreadyTranslated[i].Name}'");
                        alreadyTranslated[i].Delete();
                    }
                    Console.WriteLine();
                }
                else
                {
                    Environment.Exit(0);
                }
            }
        }
    }
}
