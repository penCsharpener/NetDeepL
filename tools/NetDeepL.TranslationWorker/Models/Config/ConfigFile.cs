using System;
using System.IO;
using System.Linq;
using NetDeepL.Models;

namespace NetDeepL.TranslationWorker.Models.Config
{
    public class ConfigFile
    {
        public ConfigFile()
        {

        }

        public ConfigFile(ConfigFileRaw raw)
        {
            DeepLApiKey = raw.DeepLApiKey;
            LanguagesToTranslate = raw.LanguagesToTranslate.Split(',', System.StringSplitOptions.RemoveEmptyEntries)
                                                            .Select(x => Enum.Parse<Languages>(x)).ToArray();
            SourceLanguage = Enum.Parse<Languages>(raw.SourceLanguage);
            DelayMilliseconds = int.Parse(raw.DelayMilliseconds);
            var currentDir = new DirectoryInfo(Directory.GetCurrentDirectory());
            InputPath = string.IsNullOrWhiteSpace(raw.InputPath) ? currentDir : new DirectoryInfo(raw.InputPath);
            OutputPath = string.IsNullOrWhiteSpace(raw.OutputPath) ? currentDir : new DirectoryInfo(raw.OutputPath);
        }

        public string DeepLApiKey { get; }
        public Languages[] LanguagesToTranslate { get; }
        public Languages SourceLanguage { get; }
        public int DelayMilliseconds { get; }
        public DirectoryInfo InputPath { get; }
        public DirectoryInfo OutputPath { get; }
    }
}
