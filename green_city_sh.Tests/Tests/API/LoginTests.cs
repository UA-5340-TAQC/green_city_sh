using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Api.Clients.GreencityUser;


namespace green_city_sh.Tests.Tests.API;

[TestFixture]
[Allure.NUnit.AllureNUnit]
[Parallelizable(ParallelScope.Self)]
public class LoginAPITests
{
    private OwnSecurityClient _client;

    [OneTimeSetUp]
    protected void OneTimeSetUp()
    {
        _client = new OwnSecurityClient(Configuration.ApiUserBaseUrl);
    }


    [Test]
    public void VerifySuccessSignIn()
    {
        var response = _client.SignIn(Configuration.TestEmail, Configuration.TestPassword);
        Assert.That(response.IsSuccessStatusCode, Is.True, "Expected successful login with valid credentials.");
        Assert.That(response.Content, Does.Contain("accessToken"), "Expected response to contain authentication token.");

    }


}
