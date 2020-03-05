using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NetDeepL.TranslationWorker.Abstractions;

namespace NetDeepL.TranslationWorker
{
    sealed class Program
    {
        private static IServiceProvider ServiceProvider { get; set; }

        static async Task Main(string[] args)
        {
            SetupDependencies();
            var translator = ServiceProvider.GetService<IWorkbookTranslator>();
            await translator.TranslateAsync();
            Console.ReadLine();
        }

        private static void SetupDependencies()
        {
            var sb = new ServiceCollection();
            var startup = new Startup();
            startup.ConfigureServices(sb);
            ServiceProvider = sb.BuildServiceProvider();
        }
    }
}
