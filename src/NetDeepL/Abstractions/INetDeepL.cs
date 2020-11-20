using System.Collections.Generic;
using System.Threading.Tasks;
using NetDeepL.Models;
using NetDeepL.Models.Parameters;

namespace NetDeepL.Abstractions
{
    public interface INetDeepL
    {
        Task<Usage> GetUsage();
        Task<TranslationReponse> TranslateAsync(string text, Languages targetLanguage);
        Task<TranslationReponse[]> TranslateAsync(IEnumerable<string> texts, Languages targetLanguage);
        Task<TranslationReponse> TranslateAsync(string text, Languages targetLanguage, bool splitSentences = true, bool preserveFormatting = false);
        Task<TranslationReponse> TranslateAsync(string text, Languages targetLanguage, Languages sourceLanguage = Languages.Undefined, bool splitSentences = true, bool preserveFormatting = false);
        Task<TranslationReponse> TranslateAsync(string text, Languages targetLanguage, TranslationRequestParameters parameters);
        Task<TranslationReponse[]> TranslateAsync(IEnumerable<string> texts, Languages targetLanguage, TranslationRequestParameters parameters);
    }
}
