using FluentAssertions;
using NetDeepL.Abstractions;
using NetDeepL.Models;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NetDeepL.Tests.IntegrationTests {

    [TestFixture]
    public class DeepApiTests {

        private INetDeepL _netDeepL;

        public DeepApiTests() {
            var userSecrets = new UserSecretProvider().GetTestSettings();
            _netDeepL = Implementations.NetDeepL.CreateClient(userSecrets.ApiKey, new NetDeepLOptions());
        }

        [Test]
        public async Task Should_Return_ApiUsage_Of_Account() {
            var usage = await _netDeepL.GetUsage();
            usage.CharacterLimit.Should().BeGreaterThan(1000000);
            usage.CharacterCount.Should().BeLessOrEqualTo(usage.CharacterLimit);
        }

        [TestCase("Hello World!")]
        public async Task Should_Translate_English_To_German(string englishText) {

            var result = await _netDeepL.TranslateAsync(englishText, Languages.DE);
            result.Should().NotBeSameAs(englishText);
        }
    }
}
