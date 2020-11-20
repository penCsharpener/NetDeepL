using System.IO;
using System.Reflection;
using NetDeepL.TranslationWorker.Abstractions;
using NetDeepL.TranslationWorker.Models.Config;

namespace NetDeepL.TranslationWorker.Implementations
{
    public class AppInformation : IAppInformation
    {
        private static PropertyInfo[] _configFileProperties = typeof(ConfigFileRaw).GetProperties();
        private readonly string _workingDir;

        public AppInformation()
        {
            _workingDir = Directory.GetCurrentDirectory();
        }

        public PropertyInfo[] GetConfigFilePropertyInfos() => _configFileProperties;

        public FileInfo[] GetExcelFiles(DirectoryInfo inputPath)
        {
            if (!inputPath.Exists)
            {
                Directory.CreateDirectory(inputPath.FullName);
            }
            return new DirectoryInfo(inputPath.FullName).GetFiles("*.xlsx", SearchOption.TopDirectoryOnly);
        }

        public string GetWorkingDirectory() => _workingDir;

    }
}
