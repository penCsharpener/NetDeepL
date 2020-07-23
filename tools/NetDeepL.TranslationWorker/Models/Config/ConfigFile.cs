﻿using System;
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
                                                            .Select(x => EnumParseWithDefault(x.ToUpper(), Languages.Undefined)).Distinct().ToArray();
            SourceLanguage = EnumParseWithDefault(raw.SourceLanguage.ToUpper(), Languages.Undefined);
            DelayMilliseconds = int.TryParse(raw.DelayMilliseconds, out var delay) ? delay : 500;
            var currentDir = new DirectoryInfo(Directory.GetCurrentDirectory());
            InputPath = string.IsNullOrWhiteSpace(raw.InputPath) ? currentDir : new DirectoryInfo(raw.InputPath);
            OutputPath = string.IsNullOrWhiteSpace(raw.OutputPath) ? currentDir : new DirectoryInfo(raw.OutputPath);
            TimeOutSeconds = int.TryParse(raw.TimeOutSeconds, out var timeout) ? timeout : 60;
        }

        public string DeepLApiKey { get; }
        public Languages[] LanguagesToTranslate { get; }
        public Languages SourceLanguage { get; }
        public int DelayMilliseconds { get; }
        public DirectoryInfo InputPath { get; }
        public DirectoryInfo OutputPath { get; }
        public int TimeOutSeconds { get; set; }

        private T EnumParseWithDefault<T>(string enumText, T defaultValue) where T : struct
        {
            try
            {
                return Enum.Parse<T>(enumText);
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
