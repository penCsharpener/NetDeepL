using System;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using NetDeepL.Abstractions;
using NetDeepL.Models;
using NetDeepL.TranslationWorker.Abstractions;

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
                    CheckForAlreadyTranslatedWorksheets(wb);

                    foreach (IXLWorksheet ws in wb.Worksheets.Where(x => x.Name.IndexOf(DEEPL_PLACEHOLDER) == -1).ToList())
                    {
                        Console.WriteLine($"Beginning worksheet '{ws.Name}'");

                        foreach (var language in conf.LanguagesToTranslate.Split(","))
                        {
                            Console.WriteLine($"Beginning Language '{language}'");

                            var translatedSheetName = $"{ws.Name}{DEEPL_PLACEHOLDER}" + language;
                            var translatedSheet = wb.Worksheets.Add(translatedSheetName);

                            foreach (IXLCell cell in ws.CellsUsed().Cast<IXLCell>())
                            {
                                var translation = await _deepL.TranslateAsync(cell.Value.ToString(), Enum.Parse<Languages>(language));
                                Console.WriteLine($"{cell.Address} \"{cell.Value}\" => \"{translation.Text}\"");
                                translatedSheet.Cell(cell.Address).Value = translation.Text;

                                // prevent status code 429 by sending request too quickly
                                await Task.Delay(delay);
                            }
                            Console.WriteLine();
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();

                    wb.Save();
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
                Console.ReadLine();
                Environment.Exit(0);
            }
        }
    }
}
