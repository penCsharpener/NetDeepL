using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NetDeepL.Abstractions;
using NetDeepL.Extensions;
using NetDeepL.Models;
using NetDeepL.Models.Parameters;

namespace NetDeepL.Implementations
{
    public partial class NetDeepL : INetDeepL
    {
        private readonly string _apiKey;
        private readonly NetDeepLOptions _options;

        internal NetDeepL(string apiKey, NetDeepLOptions options)
        {
            _apiKey = apiKey;
            _options = options;
        }

        public async Task<Usage> GetUsage()
        {
            return (await GetClient().GetUsage()).ToResponse();
        }

        public async Task<TranslationReponse> TranslateAsync(string text, Languages targetLanguage)
        {
            return (await GetClient().TranslateAsync(text, targetLanguage)).ToResponses().FirstOrDefault();
        }

        public async Task<TranslationReponse> TranslateAsync(string text, Languages targetLanguage, bool splitSentences = true, bool preserveFormatting = false)
        {
            var conf = new TranslationRequestParameters()
            {
                SplitSentences = splitSentences,
                PreserveFormatting = preserveFormatting
            };
            return (await GetClient().TranslateAsync(text, targetLanguage)).ToResponses().FirstOrDefault();
        }

        public async Task<TranslationReponse> TranslateAsync(string text, Languages targetLanguage, Languages sourceLanguage = Languages.Undefined, bool splitSentences = true, bool preserveFormatting = false)
        {
            var conf = new TranslationRequestParameters()
            {
                SourceLanguage = sourceLanguage,
                SplitSentences = splitSentences,
                PreserveFormatting = preserveFormatting
            };
            return (await GetClient().TranslateAsync(text, targetLanguage, conf)).ToResponses().FirstOrDefault();
        }

        public async Task<TranslationReponse> TranslateAsync(string text, Languages targetLanguage, Languages sourceLanguage = Languages.Undefined, Formality formality = Formality.Default, bool splitSentences = true, bool preserveFormatting = false)
        {
            var conf = new TranslationRequestParameters()
            {
                SourceLanguage = sourceLanguage,
                SplitSentences = splitSentences,
                PreserveFormatting = preserveFormatting,
                Formality = formality,
            };
            return (await GetClient().TranslateAsync(text, targetLanguage, conf)).ToResponses().FirstOrDefault();
        }

        public async Task<TranslationReponse> TranslateAsync(string text, Languages targetLanguage, TranslationRequestParameters parameters)
        {
            return (await GetClient().TranslateAsync(text, targetLanguage, parameters)).ToResponses().FirstOrDefault();
        }

        public async Task<TranslationReponse[]> TranslateAsync(IEnumerable<string> texts, Languages targetLanguage)
        {
            return (await GetClient().TranslateAsync(texts, targetLanguage)).ToResponses();
        }

        public async Task<TranslationReponse[]> TranslateAsync(IEnumerable<string> texts, Languages targetLanguage, TranslationRequestParameters parameters)
        {
            return (await GetClient().TranslateAsync(texts, targetLanguage, parameters)).ToResponses();
        }

        #region Private methods

        private IInternalClient GetClient()
        {
            return _dep.ServiceProvider.GetService<IInternalClient>();
        }

        #endregion
    }
}
