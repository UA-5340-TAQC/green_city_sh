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
    public void VerifySearchEventsUnauthorized()
    {
        RestResponse response =
            _searchEventsClient.SearchEventsWithoutAuth("event");
        
        Assert.That(
            response.StatusCode,
            Is.EqualTo(System.Net.HttpStatusCode.Unauthorized),
            "Status code should be 401 when token is missing.");
    }

    [Test]
    public void VerifySearchEventsSuccess()
    {
        RestResponse response =
            _searchEventsClient.SearchEvents(accessToken, "event");
        
        Assert.That(
            response.StatusCode,
            Is.EqualTo(System.Net.HttpStatusCode.OK),
            "Status code should be 200 for authorized search.");

        var searchResponse =
            JsonSerializer.Deserialize<SearchEventsResponseDto>(response.Content);

        Assert.Multiple(() =>
        {
            Assert.That(searchResponse, Is.Not.Null);
            Assert.That(searchResponse.page, Is.Not.Null);
            Assert.That(searchResponse.currentPage, Is.GreaterThanOrEqualTo(0));
            Assert.That(searchResponse.totalElements, Is.GreaterThanOrEqualTo(0));
        });
        
    }

}