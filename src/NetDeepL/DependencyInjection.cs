using Microsoft.Extensions.DependencyInjection;
using System;

namespace NDeepL {
    public class DependencyInjection {

        internal IServiceCollection Services { get; }
        internal IServiceProvider ServiceProvider { get; private set; }

        public DependencyInjection() {
            Services = new ServiceCollection();
        }


        public DependencyInjection Build() {
            ServiceProvider = Services.BuildServiceProvider();
            return this;
        }
    }
}
