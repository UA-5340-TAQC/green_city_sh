using green_city_sh.Tests.Api.Clients;
using green_city_sh.Tests.Api.Clients.GreencityUser;
using green_city_sh.Tests.Api.DTO;
using green_city_sh.Tests.Infrastructure;

using RestSharp;
using System.Text.Json;

namespace green_city_sh.Tests.Tests.API;

[TestFixture]
[Allure.NUnit.AllureNUnit]
[Parallelizable(ParallelScope.Self)]
public class SearchEventsAPITests
{
    private OwnSecurityClient _authClient;
    private SearchEventsClient _searchEventsClient;
    private string accessToken;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _authClient = new OwnSecurityClient(Configuration.ApiUserBaseUrl);
        _searchEventsClient = new SearchEventsClient(Configuration.ApiUserBaseUrl);

        var signInModel = new SignInModal()
        {
            email = Configuration.TestEmail,
            password = Configuration.TestPassword
        };

        var response = _authClient.SignIn(signInModel);
        var authResponse = JsonSerializer.Deserialize<AuthResponce>(response.Content);
        accessToken = authResponse.accessToken;
    }

    [Test]
    public void VerifySearchEvents_WithoutToken_Returns401()
    {
        var response =
            _searchEventsClient.SearchEventsUnauthorized("test");

        Assert.That(
            response.StatusCode,
            Is.EqualTo(System.Net.HttpStatusCode.Unauthorized),
            "Status code should be 401 when token is missing.");
    }

    [Test]
    public void VerifySearchEvents_WithUserRole_Returns403()
    {
        var response = _searchEventsClient.SearchEvents(accessToken, "event");

        Assert.That(
            response.StatusCode,
            Is.EqualTo(System.Net.HttpStatusCode.Forbidden),
            "ROLE_USER should not have access to search/events.");
    }
    
    [Test]
    public void VerifySearchEvents_EmptySearchQuery_Returns400()
    {
        var response =
            _searchEventsClient.SearchEvents(accessToken, " ");

        Assert.That(
            response.StatusCode,
            Is.EqualTo(System.Net.HttpStatusCode.BadRequest),
            "Status code should be 400 when searchQuery is empty.");
    }


}