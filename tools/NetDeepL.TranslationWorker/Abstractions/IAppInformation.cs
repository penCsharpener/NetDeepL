using System.IO;
using System.Reflection;

namespace NetDeepL.TranslationWorker.Abstractions
{
    public interface IAppInformation
    {
        PropertyInfo[] GetConfigFilePropertyInfos();
        string GetWorkingDirectory();

        FileInfo[] GetExcelFiles(DirectoryInfo inputPath);
    }
}
