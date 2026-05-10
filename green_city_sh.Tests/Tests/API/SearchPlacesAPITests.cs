using green_city_sh.Tests.Api.Clients;
using green_city_sh.Tests.Api.Clients.GreencityUser;
using green_city_sh.Tests.Api.DTO;
using green_city_sh.Tests.Infrastructure;

using NuGet.Frameworks;
using RestSharp;
using System.Text.Json;

namespace green_city_sh.Tests.Tests.API;

[TestFixture]
[Allure.NUnit.AllureNUnit]
[Parallelizable(ParallelScope.Self)]
public class SearchPlacesAPITests
{
    private OwnSecurityClient _authClient;
    private SearchClient _searchClient;
    private string accessToken;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _authClient = new OwnSecurityClient(Configuration.ApiUserBaseUrl);
        _searchClient = new SearchClient(Configuration.ApiUserBaseUrl);

        var signInModel = new SignInModal()
        {
            email = Configuration.TestEmail,
            password = Configuration.TestPassword
        };

        var response = _authClient.SignIn(signInModel);
        var authResponse = JsonSerializer.Deserialize<AuthResponce>(response.Content);
        accessToken = authResponse.accessToken;

        Console.WriteLine(response.StatusCode);
        Console.WriteLine(response.Content);
        Console.WriteLine(accessToken);
    }

    [Test]
    public void VerifySearchPlacesUnauthorized()
    {
        RestResponse response = _searchClient.SearchPlacesWithoutAuth("event");
        Assert.That(response.StatusCode,
            Is.EqualTo(System.Net.HttpStatusCode.Unauthorized),
            "Status code should be 401 when token is missing.");
        Console.WriteLine(response.Content);
    }

    [Test]
    public void VerifySearchPlacesSuccess()
    {
        RestResponse response = _searchClient.SearchPlaces(accessToken, "event");
        Console.WriteLine(response.StatusCode);
        Console.WriteLine(response.Content);
        Console.WriteLine(accessToken);

        Assert.That(response.StatusCode,
            Is.EqualTo(System.Net.HttpStatusCode.OK),
            "Status code should be 200 for authorized search.");

        var searchResponse = JsonSerializer.Deserialize<SearchPlacesResponseDto>
            (response.Content);

        Assert.Multiple(() =>
        {
            Assert.That(searchResponse, Is.Not.Null);
            Assert.That(searchResponse.page, Is.Not.Null);
            Assert.That(searchResponse.currentPage, Is.GreaterThanOrEqualTo(0));
            Assert.That(searchResponse.totalElements, Is.GreaterThanOrEqualTo(0));
        });
    }

    [Test]
    public void VerifySearchPlacesWithoutResults()
    {
        RestResponse response = _searchClient.SearchPlaces(accessToken, "qwertyuip123");
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

        var searchResponse = JsonSerializer.Deserialize<SearchPlacesResponseDto>(response.Content);
        Assert.That(searchResponse.page.Count, Is.EqualTo(0));
    }

    [Test]
    public void VerifySearchPlacesWithoutSearchQuery()
    {

        RestResponse response =
            _searchClient.SearchPlaces(accessToken, "");

        Assert.That(
            response.StatusCode,
            Is.EqualTo(System.Net.HttpStatusCode.BadRequest),
            "Status code should be 400 when searchQuery is empty.");

    }
}