using NUnit.Framework;

namespace NetDeepL.Tests
{
    public class Tests
    {


        [SetUp]
        public void Setup()
        {

        }

        [Category("Internal")]
        [Test]
        public void UserSecrets_ShouldNot_Be_Null()
        {
            var userSecrets = new UserSecretProvider();
            Assert.IsTrue(!string.IsNullOrWhiteSpace(userSecrets.GetTestSettings().ApiKey));
        }
    }
}