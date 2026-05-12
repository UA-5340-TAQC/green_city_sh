using System.Text.Json;
using Allure.Net.Commons.Attributes;
using green_city_sh.Tests.Api.Clients;
using green_city_sh.Tests.Api.Clients.GreencityUser;
using green_city_sh.Tests.Api.DTO;
using green_city_sh.Tests.Api.DTO.Habits;
using green_city_sh.Tests.Infrastructure;
using RestSharp;

namespace green_city_sh.Tests.Tests.API;

[TestFixture]
[Allure.NUnit.AllureNUnit]
[Parallelizable(ParallelScope.Fixtures)]
[AllureFeature("Habits")]
public class HabitApiTests
{
    private OwnSecurityClient _client;
    private HabitClient _habitClient;
    private long _newHabitId;

    private const string ImagePath = "TestData/test_image.jpg";

    private HabitModel CreateHabitModel() => new HabitModel
    {
        Complexity = 2,
        DefaultDuration = 30,
        HabitTranslations = new List<HabitTranslation>
        {
            new HabitTranslation
            {
                Name = "Morning Run " + Guid.NewGuid().ToString().Substring(0, 5),
                Description = "Run every morning",
                HabitItem = "Shoes",
                LanguageCode = "en"
            }
        },
        TagIds = new List<int> { 1 },
        CustomToDoListItemDto = new List<CustomToDoListItem>()
    };

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _client = new OwnSecurityClient(Configuration.ApiUserBaseUrl);
        SignInModal signInModel = new SignInModal
        {
            email = Configuration.TestEmail,
            password = Configuration.TestPassword
        };
        RestResponse response = _client.SignIn(signInModel);

        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK), "Authentication failed");
        Assert.That(response.Content, Is.Not.Null.And.Not.Empty, "Authentication response should not be empty");

        var authResponse = JsonSerializer.Deserialize<AuthResponce>(response.Content);

        Assert.That(authResponse, Is.Not.Null, "Failed to deserialize auth response");
        Assert.That(authResponse.accessToken, Is.Not.Null.And.Not.Empty, "Access token should not be empty");

        _habitClient = new HabitClient(Configuration.ApiGreenCityBaseUrl, authResponse.accessToken);
    }

    [Test]
    [AllureDescription("Add habit to favorites test")]
    public void AddHabitToFavoritesTest()
    {
        var habitRequest = CreateHabitModel();
        var createResponse = _habitClient.CreateCustomHabit(habitRequest, ImagePath);
        Assert.That(createResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Created), "Failed to create habit for favorites test");
        _newHabitId = createResponse.Data.Id;
        var favoriteResponse = _habitClient.AddHabitToFavorite(_newHabitId);
        Assert.That(favoriteResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK),
            "Status code should be 200");
    }

    [Test]
    [AllureDescription("Add habit to favorites test")]
    public void CreateCustomHabitTest()
    {
        var habitRequest = CreateHabitModel();
        var response = _habitClient.CreateCustomHabit(habitRequest, ImagePath);

        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Created),
            response.Content);

        var createdHabit = response.Data;
        _newHabitId = createdHabit.Id;

        Assert.Multiple(() =>
        {
            Assert.That(_newHabitId, Is.GreaterThan(0), "id must be greater than 0");
            Assert.That(createdHabit.Complexity, Is.EqualTo(habitRequest.Complexity));
            Assert.That(createdHabit.DefaultDuration, Is.EqualTo(habitRequest.DefaultDuration));
        });
    }

    [Test]
    [AllureDescription("Get  habits list test")]
    public void GetHabitsTest()
    {
        var response = _habitClient.GetHabits(0, 20, "en");

        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK),
            "API should return OK status");

        Assert.That(response.Data, Is.Not.Null, "Response data should not be null");
        Assert.Multiple(() =>
        {
            Assert.That(response.Data.Page, Is.Not.Null, "Page list should exist");
        });
    }

    [Test]
    [AllureDescription("Get favorites habits list test")]
    public void GetFavoritesHabitsTest()
    {
        var response = _habitClient.GetFavoriteHabits(0, 20, "en");
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK),
            "API should return OK status");
        Assert.That(response.Data, Is.Not.Null, "Response data should not be null");
        Assert.Multiple(() =>
        {
            Assert.That(response.Data.Page, Is.Not.Null, "Page list should exist");
        });

    }
    [Test]
    [AllureDescription("Remove habit from favorites test")]
    public void RemoveHabitFromFavoritesTest()
    {
        var habitRequest = CreateHabitModel();
        var createResponse = _habitClient.CreateCustomHabit(habitRequest, ImagePath);
        Assert.That(createResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Created), "Failed to create habit for favorites test");
        _newHabitId = createResponse.Data.Id;
        var favoriteResponse = _habitClient.AddHabitToFavorite(_newHabitId);
        Assert.That(favoriteResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK),
            "Status code should be 200");

        var removeFavoriteResponse = _habitClient.RemoveHabitFromFavorite(_newHabitId);
        Assert.That(removeFavoriteResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK),
            "Status code should be 200");
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        if (_newHabitId > 0)
            _habitClient.DeleteHabit(_newHabitId);
    }
}