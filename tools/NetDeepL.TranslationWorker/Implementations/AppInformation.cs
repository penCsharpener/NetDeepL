using System.IO;
using System.Reflection;
using NetDeepL.TranslationWorker.Abstractions;
using NetDeepL.TranslationWorker.Models.Config;

namespace NetDeepL.TranslationWorker.Implementations
{
    public class AppInformation : IAppInformation
    {
        private static string _workingDir = Directory.GetCurrentDirectory();
        private static PropertyInfo[] _configFileProperties = typeof(ConfigFileRaw).GetProperties();


        public PropertyInfo[] GetConfigFilePropertyInfos() => _configFileProperties;

        public FileInfo[] GetExcelFiles()
        {
            return new DirectoryInfo(_workingDir).GetFiles("*.xlsx", SearchOption.TopDirectoryOnly);
        }

        public string GetWorkingDirectory() => _workingDir;
    }
}
