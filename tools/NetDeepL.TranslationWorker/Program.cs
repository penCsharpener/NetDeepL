using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NetDeepL.TranslationWorker.Abstractions;

namespace NetDeepL.TranslationWorker
{
    sealed class Program
    {
        private static IServiceProvider ServiceProvider { get; set; }
        private static string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine($" *** NetDeepL.TranslationWorker {Version} ***");
                SetupDependencies();
                var translator = ServiceProvider.GetService<IWorkbookTranslator>();
                await translator.TranslateAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

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
