using System.Collections.Generic;
using System.Threading.Tasks;
using NetDeepL.Models;
using NetDeepL.Models.Internal;
using NetDeepL.Models.Parameters;

namespace NetDeepL.Abstractions
{
    internal interface IInternalClient
    {
        Task<InternalUsage> GetUsage();
        Task<InternalTranslationReponse> TranslateAsync(string text, Languages targetLanguage, TranslationRequestParameters parameters = null);
        Task<InternalTranslationReponse> TranslateAsync(IEnumerable<string> texts, Languages targetLanguage, TranslationRequestParameters parameters = null);
    }
}
