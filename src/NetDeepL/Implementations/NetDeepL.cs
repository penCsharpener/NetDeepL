﻿using System.Collections.Generic;
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

        public async Task<TranslationReponse> TranslateAsync(string text, Languages target_lang)
        {
            return (await GetClient().TranslateAsync(text, target_lang)).ToResponses().FirstOrDefault();
        }

        public Task<TranslationReponse> TranslateAsync(string text, Languages targetLanguage, bool splitSentences = true, bool preserveFormatting = false)
        {
            throw new System.NotImplementedException();
        }

        public Task<TranslationReponse> TranslateAsync(string text, Languages targetLanguage, Languages sourceLanguage = Languages.Undefined, bool splitSentences = true, bool preserveFormatting = false)
        {
            throw new System.NotImplementedException();
        }

        public Task<TranslationReponse> TranslateAsync(string text, Languages targetLanguage, TranslationRequestParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public Task<TranslationReponse[]> TranslateAsync(IEnumerable<string> text, Languages target_lang)
        {
            throw new System.NotImplementedException();
        }

        public Task<TranslationReponse[]> TranslateAsync(IEnumerable<string> text, Languages targetLanguage, TranslationRequestParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        #region Private methods

        private IInternalClient GetClient()
        {
            return _dep.ServiceProvider.GetService<IInternalClient>();
        }

        #endregion
    }
}
