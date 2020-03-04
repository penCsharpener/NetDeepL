using NetDeepL.Abstractions;
using NetDeepL.Models.Internal;
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
            var jsonResponse = await _httpClient.GetStreamAsync($"v2/usage?auth_key={_apiKey}");
            return await JsonSerializer.DeserializeAsync<InternalUsage>(jsonResponse);
        }

    }
}
