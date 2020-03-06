using Microsoft.Extensions.Configuration;
using NetDeepL.Tests.Models;

namespace NetDeepL.Tests
{
    public class UserSecretProvider
    {
        private readonly IConfigurationRoot _conf;
        public IConfiguration Configuration => _conf;

        public UserSecretProvider()
        {
            var confBuilder = new ConfigurationBuilder();
            confBuilder.AddUserSecrets("2f067a34-35e0-45ac-8408-feaeb67543c5");
            _conf = confBuilder.Build();
        }

        public TestSettings GetTestSettings()
        {
            var testSettings = new TestSettings();
            _conf.GetSection(nameof(TestSettings)).Bind(testSettings);
            return testSettings;
        }
    }
}
