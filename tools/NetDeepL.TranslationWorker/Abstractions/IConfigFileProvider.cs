using System.Threading.Tasks;
using NetDeepL.TranslationWorker.Models.Config;

namespace NetDeepL.TranslationWorker.Abstractions
{
    public interface IConfigFileProvider
    {
        Task<bool> CreateTemplate(bool overwrite = false);
        Task<ConfigFileRaw> GetConfig();
        Task ValidateConfigFile();
    }
}
