using NetDeepL.Models;
using NetDeepL.Models.Parameters;
using System.Threading.Tasks;

namespace NetDeepL.Implementations {
    public interface INetDeepL {
        Task<string> TranslateAsync(string text, Languages target_lang);
        Task<string> TranslateAsync(string text, Languages targetLanguage, bool splitSentences = true, bool preserveFormatting = false);
        Task<string> TranslateAsync(string text, Languages targetLanguage, Languages sourceLanguage = Languages.Undefined, bool splitSentences = true, bool preserveFormatting = false);
        Task<string> TranslateAsync(string text, Languages targetLanguage, TranslationRequestParameters parameters);
    }
}
