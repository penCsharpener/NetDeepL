﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NetDeepL.TranslationWorker.Abstractions;
using NetDeepL.TranslationWorker.Implementations;
using NetDeepL.TranslationWorker.Models.Config;

namespace NetDeepL.TranslationWorker
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfigFileProvider, ConfigFileProvider>();
            services.AddSingleton<IAppInformation, AppInformation>();
            services.AddSingleton<IWorkbookTranslator, WorkbookTranslator>();
            services.AddTransient<IOptions<ConfigFile>, ConfigFileProvider>();
            services.AddSingleton(sp =>
            {
                var configProvider = sp.GetService<IConfigFileProvider>();
                if (configProvider.CreateTemplate().Result)
                {
                    Console.WriteLine("Config template was generated. Please fill it in.");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
                var conf = configProvider.GetOptionValues();
                configProvider.ValidateConfigFile().GetAwaiter().GetResult();
                return NetDeepL.Implementations.NetDeepL.CreateClient(conf.DeepLApiKey, new NetDeepLOptions(conf.TimeOutSeconds * 1000));
            });
        }
    }
}
