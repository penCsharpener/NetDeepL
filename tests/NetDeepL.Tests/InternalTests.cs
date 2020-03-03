using NUnit.Framework;

namespace NetDeepL.Tests {
    public class Tests {


        [SetUp]
        public void Setup() {

        }

        [Ignore("Test should only run with user secrets added on developers machine.")]
        [Category("Internal")]
        [Test]
        public void UserSecrets_ShouldNot_Be_Null() {
            var userSecrets = new UserSecretProvider();
            Assert.IsTrue(!string.IsNullOrWhiteSpace(userSecrets.GetTestSettings().ApiKey));
        }
    }
}