using Allure.NUnit;
using Allure.NUnit.Attributes;
using green_city_sh.Tests.Api.Clients.Greencity;
using green_city_sh.Tests.Api.Clients.GreencityUser;
using green_city_sh.Tests.Api.DTO;
using green_city_sh.Tests.Infrastructure;
using NUnit.Framework;
using RestSharp;
using System.Net;
using System.Text.Json;

namespace green_city_sh.Tests.Tests.API;


public class HabitsCommentAPITests : BaseAPITest
{
    private HabitCommentClient _commentClient;
    private readonly int _testHabitId = 794;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var securityClient = new OwnSecurityClient(Configuration.ApiUserBaseUrl);

        var signInModel = new SignInModal
        {
            email = Configuration.TestEmail,
            password = Configuration.TestPassword
        };

        var authResponse = securityClient.SignIn(signInModel);

        Assert.That(authResponse.IsSuccessful, Is.True, $"Login failed in Setup! Status: {authResponse.StatusCode}");

        var authData = JsonSerializer.Deserialize<AuthResponce>(authResponse.Content);

        _commentClient = new HabitCommentClient(Configuration.ApiGreenCityBaseUrl, authData.accessToken);
    }

    [Test]
    [Order(1)]
    [AllureIssue("HC-1")]
    [AllureDescription("Verify that user can get active comments for a habit")]
    public void GetActiveComments_Success()
    {
        var response = _commentClient.GetActiveComments(_testHabitId);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Failed to get active comments");
        Assert.That(response.Data, Is.Not.Null, "Response data should not be null");
        Assert.That(response.Data.currentPage, Is.EqualTo(0), "Current page should be 0");
        Assert.That(response.Data.totalElements, Is.GreaterThanOrEqualTo(0), "Total elements cannot be negative");
    }

    [Test]
    [Order(2)]
    [AllureIssue("HC-2")]
    [AllureDescription("Verify that user can add a comment to a habit")]
    public void AddComment_Success()
    {
        string testCommentText = "Test Comment";
        int createdCommentId = 0;

        try
        {
            var response = _commentClient.AddComment(_testHabitId, testCommentText);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created), "Failed to create comment");
            Assert.That(response.Data, Is.Not.Null, "Response data should not be null");
            Assert.That(response.Data.text, Is.EqualTo(testCommentText), "Comment text does not match");

            createdCommentId = response.Data.id;
            Assert.That(createdCommentId, Is.GreaterThan(0), "Created comment ID should be positive");
        }
        finally
        {
            if (createdCommentId > 0)
            {
                _commentClient.DeleteComment(createdCommentId);
            }
        }
    }

    [Test]
    [Order(3)]
    [AllureIssue("HC-3")]
    [AllureDescription("Verify that user can delete own comment")]
    public void DeleteComment_Success()
    {
        var createResponse = _commentClient.AddComment(_testHabitId, "Comment for deletion test");
        Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created), "Precondition failed: Could not create comment");

        int commentIdToDelete = createResponse.Data.id;
        var deleteResponse = _commentClient.DeleteComment(commentIdToDelete);

        Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Failed to delete comment");
    }
}
