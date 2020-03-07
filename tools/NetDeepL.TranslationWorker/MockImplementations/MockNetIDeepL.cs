using System.Collections.Generic;
using System.Threading.Tasks;
using NetDeepL.Abstractions;
using NetDeepL.Models;
using NetDeepL.Models.Parameters;

namespace NetDeepL.TranslationWorker.MockImplementations
{
    public class MockNetIDeepL : INetDeepL
    {
        public Task<Usage> GetUsage()
        {
            return Task.FromResult(new Usage { CharacterCount = 0, CharacterLimit = 0 });
        }

        public Task<TranslationReponse> TranslateAsync(string text, Languages targetlanguages)
        {
            return Task.FromResult(new TranslationReponse()
            {
                DetectedSourceLanguage = Languages.Undefined,
                Text = $"translated into {targetlanguages}"
            });
        }

        public Task<TranslationReponse[]> TranslateAsync(IEnumerable<string> text, Languages targetlanguages)
        {
            return Task.FromResult(new TranslationReponse[] {
                new TranslationReponse() { DetectedSourceLanguage = Languages.Undefined, Text = $"translated into {targetlanguages}" }
            });
        }

        public Task<TranslationReponse> TranslateAsync(string text, Languages targetLanguages, bool splitSentences = true, bool preserveFormatting = false)
        {
            return Task.FromResult(new TranslationReponse()
            {
                DetectedSourceLanguage = Languages.Undefined,
                Text = $"translated into {targetLanguages}"
            });
        }

        public Task<TranslationReponse> TranslateAsync(string text, Languages targetLanguages, Languages sourceLanguage = Languages.Undefined, bool splitSentences = true, bool preserveFormatting = false)
        {
            return Task.FromResult(new TranslationReponse()
            {
                DetectedSourceLanguage = sourceLanguage,
                Text = $"translated into {targetLanguages}"
            });
        }

        public Task<TranslationReponse> TranslateAsync(string text, Languages targetLanguages, TranslationRequestParameters parameters)
        {
            return Task.FromResult(new TranslationReponse()
            {
                DetectedSourceLanguage = Languages.Undefined,
                Text = $"translated into {targetLanguages}"
            });
        }

        public Task<TranslationReponse[]> TranslateAsync(IEnumerable<string> text, Languages targetLanguages, TranslationRequestParameters parameters)
        {
            return Task.FromResult(new TranslationReponse[] {
                new TranslationReponse() { DetectedSourceLanguage = Languages.Undefined, Text = $"translated into {targetLanguages}" }
            });
        }
    }
}
