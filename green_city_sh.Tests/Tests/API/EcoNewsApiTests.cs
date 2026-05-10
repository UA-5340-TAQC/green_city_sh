using System.Text.Json;
using Allure.Net.Commons.Attributes;
using green_city_sh.Tests.Api.Clients;
using green_city_sh.Tests.Api.Clients.GreencityUser;
using green_city_sh.Tests.Api.DTO;
using green_city_sh.Tests.Api.DTO.EcoNews;
using green_city_sh.Tests.Infrastructure;
using RestSharp;

[TestFixture]
[Parallelizable(ParallelScope.Fixtures)]
[AllureOwner("Dmytro Syadro")]
[AllureFeature("Eco News")]
public class EcoNewsApiTests
{
    private OwnSecurityClient _client;
    private EcoNewsClient _ecoNewsClient;
    private long _createdNewsId;

    private const string ImagePath = "TestData/test_image.jpg";

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

        _ecoNewsClient = new EcoNewsClient(Configuration.ApiGreenCityBaseUrl, authResponse.accessToken);
    }

    [Test]
    [Order(1)]
    [AllureDescription("Verify getting eco news content by id successfully")]
    [AllureSubSuite("Eco-news-controller")]
    [AllureTag("API", "Smoke")]
    public void VerifyGettingEcoNewsByIdSuccessfully()
    {
        RestResponse<EcoNewsModel> response = _ecoNewsClient.GetEcoNewsById(10390);

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK), "Status code should be 200");
            Assert.That(response.Data, Is.Not.Null, "Response data should not be null");
            Assert.That(response.Data.Id, Is.EqualTo(10390), "News ID should match requested ID");
        });
        Assert.Multiple(() =>
        {
            Assert.That(response.Data.Title, Is.Not.Null.And.Not.Empty, "News title should not be empty");
            Assert.That(response.Data.CreationDate, Is.GreaterThan(DateTime.MinValue), "Creation date should be valid");
        });
    }

    [Test]
    [Order(2)]
    [AllureDescription("Verify creating eco news successfully")]
    [AllureSubSuite("Eco-news-controller")]
    [AllureTag("API", "Smoke")]
    public void VerifyCreatingEcoNewsSuccessfully()
    {
        var request = BuildCreateEcoNewsRequest();

        RestResponse<EcoNewsModel> response = _ecoNewsClient.CreateEcoNews(request, ImagePath);

        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Created), "Status code should be 201");
        var created = JsonSerializer.Deserialize<EcoNewsModel>(response.Content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        Assert.That(created, Is.Not.Null, "Failed to deserialize created news");
        _createdNewsId = created.Id;

        Assert.Multiple(() =>
        {
            Assert.That(response.Data?.Title, Is.EqualTo("EcoNews"), "News title should match");
            Assert.That(response.Data.ShortInfo, Is.EqualTo("Short info test"), "Short info should match");
            Assert.That(new[] { "Events", "News", "Ads" }.All(tag => response.Data.TagsEn.Contains(tag)), Is.True, "All tags should be present");
            Assert.That(response.Data.CreationDate, Is.GreaterThan(DateTime.MinValue), "Creation date should be valid");
            Assert.That(response.Data.Author.Id, Is.EqualTo(Configuration.TestUserId), "Author ID should match");
        });
    }

    [Test]
    [Order(3)]
    [AllureDescription("Verify deleting eco news by id successfully")]
    [AllureSubSuite("Eco-news-controller")]
    [AllureTag("API", "Smoke")]
    public void VerifyDeletingNewsByIdSuccessfully()
    {
        var request = BuildCreateEcoNewsRequest();

        RestResponse<EcoNewsModel> createResponse = _ecoNewsClient.CreateEcoNews(request, ImagePath);
        Assert.That(createResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Created), "Status code should be 201");

        var created = JsonSerializer.Deserialize<EcoNewsModel>(createResponse.Content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Assert.That(created, Is.Not.Null, "Failed to deserialize created news");

        RestResponse deleteResponse = _ecoNewsClient.DeleteEcoNewsById(created.Id);
        Assert.That(deleteResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK), "Status code should be 200");

        RestResponse<EcoNewsModel> getResponse = _ecoNewsClient.GetEcoNewsById(created.Id);
        Assert.That(getResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound), "Deleted news should return 404");
    }

    [Test]
    [Order(4)]
    [AllureDescription("Verify editing eco news by id successfully")]
    [AllureSubSuite("Eco-news-controller")]
    [AllureTag("API", "Smoke")]
    public void VerifyEditingEcoNewsByIdSuccessfully()
    {
        var request = BuildCreateEcoNewsRequest();

        RestResponse createResponse = _ecoNewsClient.CreateEcoNews(request, ImagePath);
        Assert.That(createResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Created), "Status code should be 201");

        var created = JsonSerializer.Deserialize<EcoNewsModel>(createResponse.Content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Assert.That(created, Is.Not.Null, "Failed to deserialize created news");
        var id = created.Id;
        _createdNewsId = id;

        var updateRequest = new UpdateEcoNewsRequest
        {
            Id = id,
            Title = "Edited EcoNews",
            Content = "Care about nature test",
            ShortInfo = "Edited Short info test",
            Tags = { "Education", "Initiatives" },
            Source = "https://www.linkedin.com"
        };

        RestResponse<EcoNewsModel> updateResponse = _ecoNewsClient.UpdateEcoNewsById(updateRequest, id, ImagePath);
        Assert.Multiple(() =>
        {
            Assert.That(updateResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK), "Status code should be 200");
            Assert.That(updateResponse.Data, Is.Not.Null, "Response data should not be null");
        });
        Assert.Multiple(() =>
        {
            Assert.That(updateResponse.Data.Title, Is.EqualTo("Edited EcoNews"), "Title should be updated");
            Assert.That(updateResponse.Data.ShortInfo, Is.EqualTo("Edited Short info test"), "Short info should be updated");
            Assert.That(new[] { "Events", "News", "Ads" }.All(tag => updateResponse.Data.TagsEn.Contains(tag)), Is.False, "Old tags should not be present");
            Assert.That(new[] { "Education", "Initiatives" }.All(tag => updateResponse.Data.TagsEn.Contains(tag)), Is.True, "New tags should be present");
        });
    }

    [Test]
    [Order(5)]
    [AllureDescription("Verify getting eco news by invalid id")]
    [AllureSubSuite("Eco-news-controller")]
    [AllureTag("API", "Smoke")]
    public void VerifyGettingEcoNewsDataByInvalidId()
    {
        RestResponse<EcoNewsModel> response = _ecoNewsClient.GetEcoNewsById(-11);

        Assert.Multiple(() =>
        {
            Assert.That(response, Is.Not.Null, "Response should not be null");
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound), "Invalid ID should return 404");
            Assert.That(response.Content, Is.Not.Null.And.Not.Empty, "Response content should not be empty");
        });
    }

    public static IEnumerable<TestCaseData> InvalidDescriptionInfoData()
    {
        yield return new TestCaseData("ShortInfo_2026@@##$");
        yield return new TestCaseData(new string('A', 63207));
        yield return new TestCaseData(new string('1', 63207));
        yield return new TestCaseData(new string('#', 63207));
        yield return new TestCaseData("a");
        yield return new TestCaseData("");
    }

    [Test, TestCaseSource(nameof(InvalidDescriptionInfoData))]
    [Order(6)]
    [AllureDescription("Verify creating eco news with an invalid data in the description field")]
    [AllureSubSuite("Eco-news-controller")]
    [AllureTag("API", "Smoke")]
    public void VerifyCreatingEcoNewsWithInvalidDescriptionInfoField(string descriptionText)
    {
        var request = new CreateEcoNewsRequest
        {
            Title = "EcoNews",
            Text = descriptionText,
            Tags = { "Events", "News", "Ads" },
            Source = "https://www.linkedin.com",
            ShortInfo = "Short info test"
        };

        RestResponse<EcoNewsModel> response = _ecoNewsClient.CreateEcoNews(request, ImagePath);
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest), "Invalid description should return 400");
            Assert.That(response.Content, Is.Not.Null.And.Not.Empty, "Response content should not be empty");
        });
    }

    public static IEnumerable<TestCaseData> InvalidTitleData()
    {
        yield return new TestCaseData(new string('a', 171));
        yield return new TestCaseData("");
    }

    [Test, TestCaseSource(nameof(InvalidTitleData))]
    [Order(7)]
    [AllureDescription("Verify editing eco news with an invalid data in the title field")]
    [AllureSubSuite("Eco-news-controller")]
    [AllureTag("API", "Smoke")]
    public void VerifyUpdatingEcoNewsWithInvalidTitleField(string titleText)
    {
        var createRequest = BuildCreateEcoNewsRequest();
        RestResponse createResponse = _ecoNewsClient.CreateEcoNews(createRequest, ImagePath);
        Assert.That(createResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Created), "Status code should be 201");

        var created = JsonSerializer.Deserialize<EcoNewsModel>(createResponse.Content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        Assert.That(created, Is.Not.Null, "Failed to deserialize created news");
        var id = created.Id;
        _createdNewsId = created.Id;

        var updateRequest = new UpdateEcoNewsRequest
        {
            Id = id,
            Title = titleText,
            Content = "Care about nature test",
            Tags = { "Events", "News", "Ads" },
            Source = "https://www.linkedin.com",
            ShortInfo = "Short info test"
        };
        RestResponse response = _ecoNewsClient.UpdateEcoNewsById(updateRequest, id, ImagePath);
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest), "Invalid title should return 400");
            Assert.That(response.Content, Is.Not.Null.And.Not.Empty, "Response content should not be empty");
        });
    }

    [Test]
    [Order(8)]
    [AllureDescription("Verify creating eco news being unauthorized")]
    [AllureSubSuite("Eco-news-controller")]
    [AllureTag("API", "Smoke")]
    public void VerifyCreatingEcoNewsBeingUnauthorized()
    {
        var unauthorized = new EcoNewsClient(Configuration.ApiGreenCityBaseUrl);
        var createRequest = BuildCreateEcoNewsRequest();

        RestResponse response = unauthorized.CreateEcoNews(createRequest, ImagePath);
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized), "Unauthorized request should return 401");
            Assert.That(response.Content, Is.Not.Null.And.Not.Empty, "Response content should not be empty");
        });
    }

    [Test]
    [Order(9)]
    [AllureDescription("Verify adding eco news to favorites being unauthorized")]
    [AllureSubSuite("Eco-news-controller")]
    [AllureTag("API", "Smoke")]
    public void VerifyAddingEcoNewsToFavoritesNewsBeingUnauthorized()
    {
        var unauthorized = new EcoNewsClient(Configuration.ApiGreenCityBaseUrl);
        var response = unauthorized.AddEcoNewsToFavorites(10405);
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized), "Unauthorized request should return 401");
            Assert.That(response.Content, Is.Not.Null.And.Not.Empty, "Response content should not be empty");
        });
    }

    private CreateEcoNewsRequest BuildCreateEcoNewsRequest() => new CreateEcoNewsRequest
    {
        Title = "EcoNews",
        Text = "Care about nature test",
        Tags = { "Events", "News", "Ads" },
        Source = "https://www.linkedin.com",
        ShortInfo = "Short info test"
    };

    [TearDown]
    public void TearDown()
    {
        if (_createdNewsId > 0)
        {
            _ecoNewsClient.DeleteEcoNewsById(_createdNewsId);
            _createdNewsId = 0;
        }
    }
}