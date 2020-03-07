using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NetDeepL.Abstractions;
using NetDeepL.Models;
using NUnit.Framework;

namespace NetDeepL.Tests.IntegrationTests
{

    [TestFixture]
    public class DeepApiTests
    {

        private INetDeepL _netDeepL;

        public DeepApiTests()
        {
            var userSecrets = new UserSecretProvider().GetTestSettings();
            _netDeepL = Implementations.NetDeepL.CreateClient(userSecrets.ApiKey, new NetDeepLOptions());
        }

        [Test]
        public async Task Should_Return_ApiUsage_Of_Account()
        {
            var usage = await _netDeepL.GetUsage();
            usage.CharacterLimit.Should().BeGreaterThan(1000000);
            usage.CharacterCount.Should().BeLessOrEqualTo(usage.CharacterLimit);
        }

        [TestCase("Hello world!")]
        public async Task Should_Translate_English_To_German(string englishText)
        {
            var result = await _netDeepL.TranslateAsync(englishText, Languages.DE);
            result.Text.Should().NotBeNullOrWhiteSpace();
            result.Should().NotBeSameAs(englishText);
        }

        [Test]
        public async Task Should_Translate_Multiple_English_To_German()
        {
            var texts = new List<string>() {
                "Once upon a time there was an artifical intelligence.",
                "It could translate multiple sentences in a single API request.",
                "This service was called DeepL."
            };

            var results = await _netDeepL.TranslateAsync(texts, Languages.DE);
            for (int i = 0; i < results.Length; i++)
            {
                results[i].Text.Should().NotBeNullOrWhiteSpace();
            }
            results[0].Text.Should().Contain("künstliche");
            results[1].Text.Should().Contain("API-Anfrage");
            results[2].Text.Should().Contain("genannt");
        }
    }
}
