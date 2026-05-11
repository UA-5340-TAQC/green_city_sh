using NUnit.Framework;
using RestSharp;
using System.Net;
using System.Text.Json;
using System;
using green_city_sh.Tests.Api.Clients;
using green_city_sh.Tests.Api.DTO;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Api.Clients.GreencityUser;
using Allure.Net.Commons.Attributes;

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
        private int createdCommentId;

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
            string validToken = authData.accessToken;

            authorizedClient = new EventCommentClient(ApiBaseUrl, validToken);
        }

        [Test, Order(1)]
        [AllureDescription("Verify that comments returns successfully")]
        public void GetEventComments()
        {
            var response = unauthorizedClient.GetComments(TestEventId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content, Is.Not.Null);
        }

        [Test, Order(2)]
        [AllureDescription("Verify that authorized user can add comment to event")]
        public void AddCommentAuthorizedUser()
        {
            var payload = new AddCommentRequest
            {
                text = "This is a test comment from API framework",
                parentCommentId = 0
            };

            var response = authorizedClient.AddComment(TestEventId, payload);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created), $"Server returned {response.StatusCode}");

            var responseData = JsonSerializer.Deserialize<AddCommentResponse>(response.Content);
            createdCommentId = responseData.id;
        }

        [Test, Order(3)]
        [AllureDescription("Verify that authorized user can delete their comment")]
        public void DeleteCommentAuthorizedUser()
        {
            Assert.That(createdCommentId, Is.GreaterThan(0), "Cannot delete: Comment ID was not captured in the POST test.");

            var response = authorizedClient.DeleteComment(createdCommentId);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}