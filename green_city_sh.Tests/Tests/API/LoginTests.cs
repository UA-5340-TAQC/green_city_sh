using Allure.Net.Commons.Attributes;
using green_city_sh.Tests.Api.Clients.GreencityUser;
using green_city_sh.Tests.Api.DTO;
using green_city_sh.Tests.Infrastructure;
using NuGet.Frameworks;
using RestSharp;
using System.Text.Json;


namespace green_city_sh.Tests.Tests.API;


[AllureSubSuite("Login API Tests")]
public class LoginAPITests : BaseAPITest
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
        RestResponse response = _client.SignIn(signInModel);

        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK), "Status code should be 200 for successful sign in.");
        AuthResponce authResponse = JsonSerializer.Deserialize<AuthResponce>(response.Content);
        Assert.Multiple(() =>
        {
            Assert.That(Configuration.TestUserId, Is.EqualTo(authResponse.userId), "User ID does not match expected value.");
            Assert.IsFalse(string.IsNullOrEmpty(authResponse.accessToken), "Access token should not be null or empty.");
            Assert.IsFalse(string.IsNullOrEmpty(authResponse.refreshToken), "Refresh token should not be null or empty.");
            Assert.That(Configuration.TestUserName, Is.EqualTo(authResponse.name), "User name does not match expected value.");
            Assert.IsTrue(authResponse.ownRegistrations, "Own registrations should be true for the test user.");
        });

    }

    [Test]
    public void VerifySignInNotValidPassword()
    {

        SignInModal signInModel = new SignInModal
        {
            email = Configuration.TestEmail,
            password = Configuration.TestPassword + "invalid"
        };
        RestResponse response = _client.SignIn(signInModel);

        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest), "Status code should be 400 for invalid password.");
        ErrorResponceNameMesageDto errorResponce = JsonSerializer.Deserialize<ErrorResponceNameMesageDto>(response.Content);
        Assert.Multiple(() =>
        {
            Assert.That(errorResponce.name, Is.EqualTo("password"), "Error name should be 'password' for invalid password.");
            Assert.That(errorResponce.message, Is.EqualTo("Bad password"), "Error message should be 'Bad password' for invalid password.");
        });

    }


}
