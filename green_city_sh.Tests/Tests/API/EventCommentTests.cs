using NUnit.Framework;
using RestSharp;
using System.Net;
using System.Text.Json;
using System;
using green_city_sh.Tests.Api.DTO;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Api.Clients.GreencityUser;
using Allure.Net.Commons.Attributes;
using green_city_sh.Tests.Api.Clients.Greencity;

namespace green_city_sh.Tests.Tests.API
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    [Allure.NUnit.AllureNUnit]
    [AllureFeature("Event Comments API Tests")]
    public class EventCommentApiTests
    {
        private EventCommentClient unauthorizedClient;
        private EventCommentClient authorizedClient;

        public static string ApiBaseUrl => Configuration.ApiGreenCityBaseUrl;
        private const int TestEventId = 50;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            unauthorizedClient = new EventCommentClient(ApiBaseUrl);
            var authClient = new OwnSecurityClient(Configuration.ApiUserBaseUrl);

            var credentials = new SignInModal
            {
                email = Configuration.TestEmail,
                password = Configuration.TestPassword
            };

            RestResponse loginResponse = authClient.SignIn(credentials);

            if (loginResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Pre-condition failed: Could not log in to get JWT. Status Code: {loginResponse.StatusCode}. Message: {loginResponse.Content}");
            }

            AuthResponce authData = JsonSerializer.Deserialize<AuthResponce>(loginResponse.Content);
            Assert.That(authData, Is.Not.Null, "Failed to deserialize AuthResponce. Body might be empty.");
            string validToken = authData.accessToken;

            authorizedClient = new EventCommentClient(ApiBaseUrl, validToken);
        }

        [Test]
        [AllureDescription("Verify that comments returns successfully")]
        public void GetEventComments()
        {
            var response = unauthorizedClient.GetComments(TestEventId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content, Is.Not.Null);
        }

        [Test]
        [AllureDescription("Verify that authorized user can add comment to event")]
        public void AddCommentAuthorizedUser()
        {
            var payload = new AddCommentRequest
            {
                text = "This is a test comment from API framework",
                parentCommentId = 0
            };

            int newCommentId = 0;

            try
            {
                var response = authorizedClient.AddComment(TestEventId, payload);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created), $"Server returned {response.StatusCode}");

                var responseData = JsonSerializer.Deserialize<AddCommentResponse>(response.Content);
                Assert.That(responseData, Is.Not.Null, "Deserialized response data is null. Cannot extract comment ID.");

                newCommentId = responseData.id;
            }
            finally
            {

                if (newCommentId > 0)
                {
                    authorizedClient.DeleteComment(newCommentId);
                }
            }
        }

        [Test]
        [AllureDescription("Verify that authorized user can't like their own comment")]
        public void UnableLikeCommentAuthorizedUser()
        {
            int commentId = CreateIsolatedTestComment("Comment for Like test");

            try
            {
                var response = authorizedClient.LikeComment(commentId);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest),
                    $"Expected server to reject the like, but got {response.StatusCode}. Content: {response.Content}");
            }
            finally
            {
                authorizedClient.DeleteComment(commentId);
            }
        }

        [Test]
        [AllureDescription("Verify that authorized user can update their comment")]
        public void UpdateCommentAuthorizedUser()
        {
            int commentId = CreateIsolatedTestComment("Original comment text");
            string updatedText = "This comment was updated by automated API test!";

            try
            {
                var response = authorizedClient.UpdateComment(commentId, updatedText);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK),
                    $"Server returned {response.StatusCode}. Details: {response.Content}");
            }
            finally
            {
                authorizedClient.DeleteComment(commentId);
            }
        }

        [Test]
        [AllureDescription("Verify that authorized user can delete their comment")]
        public void DeleteCommentAuthorizedUser()
        {
            int commentId = CreateIsolatedTestComment("Comment to be deleted");

            var response = authorizedClient.DeleteComment(commentId);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        private int CreateIsolatedTestComment(string text)
        {
            var payload = new AddCommentRequest { text = text, parentCommentId = 0 };
            var response = authorizedClient.AddComment(TestEventId, payload);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created),
                $"Pre-condition failed: Could not create test comment. Status: {response.StatusCode}");

            var responseData = JsonSerializer.Deserialize<AddCommentResponse>(response.Content);
            Assert.That(responseData, Is.Not.Null,
                "Pre-condition failed: Deserialized response data is null.");

            return responseData.id;
        }
    }
}