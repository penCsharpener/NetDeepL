using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NetDeepL.TranslationWorker.Models.Config;

namespace NetDeepL.TranslationWorker.Abstractions
{
    public interface IConfigFileProvider : IOptions<ConfigFile>
    {
        Task<bool> CreateTemplate(bool overwrite = false);
        Task<ConfigFileRaw> GetConfig();
        Task ValidateConfigFile();
        ConfigFile GetOptionValues();
    }
}
