using System;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using NetDeepL.Abstractions;
using NetDeepL.Models;
using NetDeepL.TranslationWorker.Abstractions;
using NetDeepL.TranslationWorker.Models;

namespace NetDeepL.TranslationWorker.Implementations
{
    public class WorkbookTranslator : IWorkbookTranslator
    {
        private const string DEEPL_PLACEHOLDER = "_DeepL_";
        private readonly IAppInformation _appInformation;
        private readonly INetDeepL _deepL;
        private readonly IConfigFileProvider _configFileProvider;

        public WorkbookTranslator(IAppInformation appInformation, INetDeepL deepL, IConfigFileProvider configFileProvider)
        {
            _appInformation = appInformation;
            _deepL = deepL;
            _configFileProvider = configFileProvider;
        }

        public async Task TranslateAsync()
        {
            var conf = await _configFileProvider.GetConfig();
            var delay = int.Parse(conf.DelayMilliseconds);

            var usageBefore = await _deepL.GetUsage();
            Console.WriteLine();
            Console.WriteLine("Translation starting");
            Console.WriteLine($"Api usage: {usageBefore.CharacterCount}/{usageBefore.CharacterLimit}");
            Console.WriteLine();

            foreach (var xlsx in _appInformation.GetExcelFiles())
            {
                Console.WriteLine();
                Console.WriteLine($"processing file '{xlsx.Name}'");

                using (var wb = new XLWorkbook(xlsx.FullName))
                {
                    try
                    {
                        CheckForAlreadyTranslatedWorksheets(wb);

                        await ProcessSheets(wb, conf.LanguagesToTranslate, delay);
                        Console.WriteLine();

                        wb.Save();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }

            Console.WriteLine();
            var usageAfter = await _deepL.GetUsage();
            Console.WriteLine("Translation finished");
            Console.WriteLine($"Api usage: {usageAfter.CharacterCount}/{usageAfter.CharacterLimit}");
            Console.WriteLine();
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

        private async Task ProcessSheets(IXLWorkbook workbook, string languagesToTranslate, int delay)
        {
            foreach (IXLWorksheet ws in workbook.Worksheets.Where(x => x.Name.IndexOf(DEEPL_PLACEHOLDER) == -1).ToList())
            {
                Console.WriteLine($"Beginning worksheet '{ws.Name}'");

                foreach (var language in languagesToTranslate.Split(","))
                {
                    var langEnum = Enum.Parse<Languages>(language);
                    Console.WriteLine($"Language '{language}'");

                    var usedCells = ws.CellsUsed().Cast<IXLCell>()
                                                  .Where(x => x.Value != null)
                                                  .Select(x => new ExcelCell(x))
                                                  .ToList();

                    // don't create new sheet for sheets that are empty
                    if (usedCells.Count > 0)
                    {
                        var translatedSheet = workbook.Worksheets.Add($"{ws.Name}{DEEPL_PLACEHOLDER}{language}");

                        foreach (var cell in usedCells)
                        {
                            await TranslateCell(cell, translatedSheet, delay, langEnum);
                        }

                        Console.WriteLine();
                    }
                }
                Console.WriteLine();
            }
        }

        private async Task TranslateCell(ExcelCell cell, IXLWorksheet translatedSheet, int delay, Languages language)
        {
            var translation = cell.Text;
            if (!cell.IsHidden)
            {
                translation = (await _deepL.TranslateAsync(cell.Text, language)).Text;
            }

            Console.WriteLine($"{cell.Address} \"{cell.Text}\" => \"{translation}\"");
            translatedSheet.Cell(cell.Address).Value = translation;

            // prevent status code 429 by sending request too quickly
            await Task.Delay(delay);
        }
    }
}
