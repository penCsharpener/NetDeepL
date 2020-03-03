using NetDeepL.Abstractions;
using NetDeepL.Models;
using NetDeepL.Models.Parameters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetDeepL.Implementations {
    public partial class NetDeepL : INetDeepL {
        private readonly string _apiKey;
        private readonly NetDeepLOptions _options;

        internal NetDeepL(string apiKey, NetDeepLOptions options) {
            _apiKey = apiKey;
            _options = options;
        }

        public Task<string> TranslateAsync(string text, Languages target_lang) {
            throw new System.NotImplementedException();
        }

        public Task<string> TranslateAsync(string text, Languages targetLanguage, bool splitSentences = true, bool preserveFormatting = false) {
            throw new System.NotImplementedException();
        }

        public Task<string> TranslateAsync(string text, Languages targetLanguage, Languages sourceLanguage = Languages.Undefined, bool splitSentences = true, bool preserveFormatting = false) {
            throw new System.NotImplementedException();
        }

        public Task<string> TranslateAsync(string text, Languages targetLanguage, TranslationRequestParameters parameters) {
            throw new System.NotImplementedException();
        }

        public Task<string> TranslateAsync(IEnumerable<string> text, Languages target_lang) {
            throw new System.NotImplementedException();
        }

        public Task<string> TranslateAsync(IEnumerable<string> text, Languages targetLanguage, TranslationRequestParameters parameters) {
            throw new System.NotImplementedException();
        }
    }
}
