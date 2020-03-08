using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NetDeepL.TranslationWorker.Abstractions;
using NetDeepL.TranslationWorker.Models.Config;

namespace NetDeepL.TranslationWorker.Implementations
{
    public class ConfigFileProvider : IConfigFileProvider
    {
        private readonly IAppInformation _appInformation;
        private const string CONFIG_FILENAME = "config.ini";
        private string _configFullFilePath;

        public ConfigFile Value => GetOptionValues();

        public ConfigFileProvider(IAppInformation appInformation)
        {
            _appInformation = appInformation;
            _configFullFilePath = Path.Combine(_appInformation.GetWorkingDirectory(), CONFIG_FILENAME);
        }

        public async Task<bool> CreateTemplate(bool overwrite = false)
        {
            if (File.Exists(_configFullFilePath) && overwrite)
            {
                File.Delete(_configFullFilePath);
            }

            if (!File.Exists(_configFullFilePath) || overwrite)
            {
                var conf = new ConfigFileRaw();
                var lines = new List<string>();
                foreach (var propInfo in _appInformation.GetConfigFilePropertyInfos())
                {
                    var value = propInfo.GetValue(conf);
                    var name = propInfo.Name;
                    lines.Add($"{name}={value}");
                }

                await File.WriteAllLinesAsync(_configFullFilePath, lines);
                return true;
            }
            return false;
        }

        public async Task<ConfigFileRaw> GetConfig()
        {
            var conf = new ConfigFileRaw();
            if (File.Exists(_configFullFilePath))
            {
                var lines = await File.ReadAllLinesAsync(_configFullFilePath);
                var props = _appInformation.GetConfigFilePropertyInfos();
                foreach (var line in lines)
                {
                    var propName = line.Substring(0, line.IndexOf("="));
                    var propValueComment = line.Replace($"{propName}=", "");
                    if (!string.IsNullOrWhiteSpace(propName) && !string.IsNullOrWhiteSpace(propValueComment))
                    {
                        var value = propValueComment;

                        // remove comments in config file
                        if (propValueComment.Contains("#"))
                        {
                            value = propValueComment.Split("#")[0].Trim();
                        }

                        var propertyInfo = Array.Find(props, x => x.Name.IndexOf(propName, System.StringComparison.OrdinalIgnoreCase) >= 0);
                        propertyInfo.SetValue(conf, value);
                    }
                }
            }

            return conf;
        }

        public async Task ValidateConfigFile()
        {
            var conf = await GetConfig();
            var configFileInvalid = false;
            conf.SourceLanguage = conf.SourceLanguage.ToUpper();
            conf.LanguagesToTranslate = string.Join(",", conf.LanguagesToTranslate.ToUpper().Split(",").Distinct());

            if (conf.SourceLanguage.Length != 2)
            {
                Console.WriteLine("Check your source language. Only one source language is allowed and it must be two letter.");
                configFileInvalid = true;
            }

            if (configFileInvalid)
            {
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        public ConfigFile GetOptionValues()
        {
            return new ConfigFile(GetConfig().Result);
        }
    }
}
