using System;
using System.Net;
using System.Text.Json;
using Allure.Net.Commons.Attributes;
using green_city_sh.Tests.Api.Clients.GreencityUser;
using green_city_sh.Tests.Api.DTO;
using green_city_sh.Tests.Infrastructure;
using NUnit.Framework;
using RestSharp;

namespace green_city_sh.Tests.Tests.API
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    [Allure.NUnit.AllureNUnit]
    [AllureFeature("Friend API Tests")]
    public class FriendTests
    {
        private FriendClient authorizedClient;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var authClient = new OwnSecurityClient(Configuration.ApiUserBaseUrl);

            var credentials = new SignInModal
            {
                email = Configuration.TestEmail,
                password = Configuration.TestPassword
            };

            RestResponse loginResponse = authClient.SignIn(credentials);

            if (loginResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(
                    $"Pre-condition failed: Could not log in to get JWT. " +
                    $"Status Code: {loginResponse.StatusCode}. Message: {loginResponse.Content}");
            }

            AuthResponce authData =
                JsonSerializer.Deserialize<AuthResponce>(loginResponse.Content!)!;

            Assert.That(authData, Is.Not.Null, "Failed to deserialize AuthResponce.");
            Assert.That(authData.accessToken, Is.Not.Null.And.Not.Empty, "Access token is empty.");

            string validToken = authData.accessToken;

            authorizedClient = new FriendClient(
                Configuration.ApiGreenCityBaseUrl,
                validToken);
        }

        [Test, Order(1)]
        [AllureDescription("Verify that authorized user can get friends list")]
        public void VerifyGetFriends()
        {
            RestResponse response = authorizedClient.GetFriends();

            Assert.That(
                response.StatusCode,
                Is.EqualTo(HttpStatusCode.OK),
                $"Server returned {response.StatusCode}. Details: {response.Content}");

            Assert.That(response.Content, Is.Not.Null.And.Not.Empty);

            FriendsPageResponse friendsResponse =
                JsonSerializer.Deserialize<FriendsPageResponse>(response.Content!)!;

            Assert.Multiple(() =>
            {
                Assert.That(friendsResponse, Is.Not.Null);
                Assert.That(friendsResponse.page, Is.Not.Null);
                Assert.That(friendsResponse.totalElements, Is.GreaterThanOrEqualTo(0));
                Assert.That(friendsResponse.currentPage, Is.EqualTo(0));
                Assert.That(friendsResponse.totalPages, Is.GreaterThanOrEqualTo(0));
            });
        }

        [Test, Order(2)]
        [AllureDescription("Verify that user cannot add already existing friend")]
        public void VerifyCannotAddAlreadyExistingFriend()
        {
            long existingFriendId = 1713;

            RestResponse response = authorizedClient.AddFriend(existingFriendId);

            Assert.That(
                response.StatusCode,
                Is.EqualTo(HttpStatusCode.BadRequest),
                $"Server returned {response.StatusCode}. Details: {response.Content}");

            Assert.That(
                response.Content,
                Does.Contain("Friend with this id has already been added"));
        }

        [Test, Order(3)]
        [AllureDescription("Verify that authorized user can add friend")]
        public void VerifyAddFriend()
        {
            long friendId = 455;

            // cleanup possible pending request
            authorizedClient.CancelFriendRequest(friendId);

            // cleanup possible existing friend
            authorizedClient.DeleteFriend(friendId);

            // add friend
            RestResponse addResponse =
                authorizedClient.AddFriend(friendId);

            Assert.That(
                addResponse.StatusCode,
                Is.EqualTo(HttpStatusCode.OK),
                $"Add friend failed. Server returned {addResponse.StatusCode}. Details: {addResponse.Content}");

            // cleanup after test
            authorizedClient.CancelFriendRequest(friendId);
            authorizedClient.DeleteFriend(friendId);
        }

    }
}
