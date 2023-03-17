using Microsoft.AspNetCore.Http;
using ThreaditAPI.Extensions;

namespace ThreaditTests.Repositories;

public class HttpContextExtensionsTests
{

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void UserSettingsInvalidTest() {
        HttpContext ctx = new DefaultHttpContext();
        Assert.Throws<Exception>(() => { ctx.GetUser(); });
    }
}