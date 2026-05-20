using green_city_sh.Tests.Api.Clients.Greencity;
using green_city_sh.Tests.Api.Clients.GreencityUser;
using green_city_sh.Tests.Api.DTO;
using green_city_sh.Tests.Infrastructure;

using System.Text.Json;

namespace green_city_sh.Tests.Tests.API;

public class SearchPlacesAPITests : BaseAPITest
{
    private OwnSecurityClient _authClient;
    private SearchClient _searchClient;
    private string accessToken;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _authClient = new OwnSecurityClient(Configuration.ApiUserBaseUrl);
        _searchClient = new SearchClient(Configuration.ApiUserBaseUrl);

        var response = _authClient.SignIn(new SignInModal()
        {
            email = Configuration.TestEmail,
            password = Configuration.TestPassword
        });

        accessToken = JsonSerializer.Deserialize<AuthResponce>(response.Content).accessToken;
    }

    [Test]
    public void VerifySearchPlaces_Unauthorized_Returns401()
    {
        var response = _searchClient.SearchPlacesWithoutAuth("test");
        Assert.That(response.StatusCode,
            Is.EqualTo(System.Net.HttpStatusCode.Unauthorized),
            "Without token search should return 401");
    }

    [Test]
    public void VerifySearchPlaces_WithUserRole_Returns403()
    {
        var response = _searchClient.SearchPlaces(accessToken, "test");

        Assert.That(response.StatusCode,
            Is.EqualTo(System.Net.HttpStatusCode.Forbidden),
            "ROLE_USER should not have access to search/places");
    }
}
