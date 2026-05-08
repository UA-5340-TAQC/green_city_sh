using green_city_sh.Tests.Api.Clients.GreencityUser;
using green_city_sh.Tests.Api.DTO;
using green_city_sh.Tests.Infrastructure;
using NuGet.Frameworks;


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

        SignInModal signInModel = new SignInModal
        {
            email = Configuration.TestEmail,
            password = Configuration.TestPassword
        };
        AuthResponce response = _client.SignIn(signInModel).Result;

        Assert.IsNotNull(response);
        Assert.Multiple(() =>
        {
            Assert.That(Configuration.TestUserId, Is.EqualTo(response.userId), "User ID does not match expected value.");
            Assert.IsFalse(string.IsNullOrEmpty(response.accessToken), "Access token should not be null or empty.");
            Assert.IsFalse(string.IsNullOrEmpty(response.refreshToken), "Refresh token should not be null or empty.");
            Assert.That(Configuration.TestUserName, Is.EqualTo(response.name), "User name does not match expected value.");
            Assert.IsTrue(response.ownRegistrations, "Own registrations should be true for the test user.");
        });

    }


}
