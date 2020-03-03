using Microsoft.Extensions.DependencyInjection;
using NetDeepL.Abstractions;
using System;

namespace NetDeepL {
    internal class DependencyInjection {

        internal IServiceCollection Services { get; }
        internal IServiceProvider ServiceProvider { get; private set; }

        internal DependencyInjection() {
            Services = new ServiceCollection();
        }

        private DependencyInjection AddHttpClient(double timeOut) {
            Services.AddHttpClient("DeepLClient", http => {
                http.BaseAddress = new Uri("api.deepl.com/v2");
                http.Timeout = TimeSpan.FromMilliseconds(timeOut);
            });
            return this;
        }

        private DependencyInjection WireUpServices(string apiKey, NetDeepLOptions options) {
            Services.AddTransient<INetDeepL, Implementations.NetDeepL>(_ => new Implementations.NetDeepL(apiKey, options));
            return this;
        }

        internal DependencyInjection Build() {
            ServiceProvider = Services.BuildServiceProvider();
            return this;
        }

        internal INetDeepL GetClient(string apiKey, NetDeepLOptions options) {
            this.AddHttpClient(options.TimeOut)
                .WireUpServices(apiKey, options)
                .Build();

            return ServiceProvider.GetService<INetDeepL>();
        }
    }
}
