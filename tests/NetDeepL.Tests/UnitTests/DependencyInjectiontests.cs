using NetDeepL.Abstractions;
using NUnit.Framework;

namespace NetDeepL.Tests.UnitTests {

    [TestFixture]
    public class DependencyInjectiontests {


        public DependencyInjectiontests() {

        }

        [TestCase()]
        public void CreateClient_Should_Not_Throw_Exception() {
            INetDeepL client;
            Assert.DoesNotThrow(() => client = Implementations.NetDeepL.CreateClient("123", new NetDeepLOptions()));
        }
    }
}
