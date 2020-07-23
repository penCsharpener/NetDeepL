using System;
using System.IO;

namespace NetDeepL.TranslationWorker.Models.Config
{
    public class ConfigFileRaw
    {
        public ConfigFileRaw()
        {
            var currentDir = Directory.GetCurrentDirectory();
            OutputPath = $"{currentDir}\\output";
            InputPath = $"{currentDir}\\input";
        }

        public string DeepLApiKey { get; set; } = Guid.Empty.ToString();
        public string LanguagesToTranslate { get; set; } = "EN,DE,FR,ES,PT,IT,NL,PL,RU # multiple possible";
        public string SourceLanguage { get; set; } = "EN,DE,FR,ES,PT,IT,NL,PL,RU # only select one language";
        public string DelayMilliseconds { get; set; } = "500 # 1 second = 1000 millisecond, if this value is too low DeepL could response with code 429";
        public string InputPath { get; set; }
        public string OutputPath { get; set; }
        public string TimeOutSeconds { get; set; } = "60";
    }
}
