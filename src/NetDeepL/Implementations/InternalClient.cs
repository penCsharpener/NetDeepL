using NetDeepL.Abstractions;
using NetDeepL.Models;
using NetDeepL.Models.Internal;
using NetDeepL.Models.Parameters;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace NetDeepL.Implementations {

    internal class InternalClient : IInternalClient {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public InternalClient(IHttpClientFactory clientFactory, IUrlBuilder urlBuilder, string apiKey) {
            _httpClient = clientFactory.CreateClient(Constants.DeepLHttpClient);
            _apiKey = apiKey;
        }

        public async Task<InternalUsage> GetUsage() {
            var responseStream = await _httpClient.GetStreamAsync($"v2/usage?auth_key={_apiKey}");
            return await JsonSerializer.DeserializeAsync<InternalUsage>(responseStream);
        }

        public async Task<InternalTranslationReponse> TranslateAsync(string text, Languages target_lang, TranslationRequestParameters parameters = null) {
            var dict = new Dictionary<string, string>(); //&text={text}&target_lang={target_lang.ToString()}
            dict.Add("text", text);
            dict.Add("target_lang", target_lang.ToString());
            var httpResponse = await _httpClient.SendAsync(GetPostRequestObject($"v2/translate?auth_key={_apiKey}", dict));
            var stream = await httpResponse.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<InternalTranslationReponse>(stream);
        }

        private HttpRequestMessage GetPostRequestObject(string url, Dictionary<string, string> dict) {
            var req = new HttpRequestMessage(HttpMethod.Post, url) {
                Content = new FormUrlEncodedContent(dict)
            };
            req.Content.Headers.Clear();
            req.Content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            return req;
        }

    }

    internal class HttpHeader {
        public HttpHeader(string headerName, string content) {
            HeaderName = headerName;
            Content = content;
        }

        public string HeaderName { get; set; }
        public string Content { get; set; }
    }
}
