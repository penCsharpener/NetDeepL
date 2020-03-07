using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using NetDeepL.Abstractions;
using NetDeepL.Implementations;

namespace NetDeepL
{
    internal class DependencyInjection
    {

        internal IServiceCollection Services { get; }
        internal IServiceProvider ServiceProvider { get; private set; }

        internal DependencyInjection()
        {
            Services = new ServiceCollection();
        }

        private DependencyInjection AddHttpClient(double timeOut)
        {
            Services.AddHttpClient(Constants.DeepLHttpClient, http =>
            {
                http.BaseAddress = new Uri("https://api.deepl.com");
                http.Timeout = TimeSpan.FromMilliseconds(timeOut);
            });
            return this;
        }

        private DependencyInjection WireUpServices(string apiKey, NetDeepLOptions options)
        {
            Services.AddTransient<INetDeepL, Implementations.NetDeepL>(_ => new Implementations.NetDeepL(apiKey, options));
            Services.AddTransient<IUrlBuilder, UrlBuilder>();
            Services.AddTransient<IInternalClient, InternalClient>(c => new InternalClient(c.GetService<IHttpClientFactory>(),
                                                                                           c.GetService<IUrlBuilder>(),
                                                                                           apiKey));
            return this;
        }

        internal DependencyInjection Build()
        {
            ServiceProvider = Services.BuildServiceProvider();
            return this;
        }

        internal INetDeepL GetClient(string apiKey, NetDeepLOptions options)
        {
            this.AddHttpClient(options.TimeOut)
                .WireUpServices(apiKey, options)
                .Build();

            return ServiceProvider.GetService<INetDeepL>();
        }
    }
}
