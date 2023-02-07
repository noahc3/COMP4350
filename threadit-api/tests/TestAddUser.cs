using ThreaditAPI.Services;

namespace Prime.UnitTests.Services
{
    [TestFixture]
    public class Tests
    {
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            _userService = new UserService();
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}