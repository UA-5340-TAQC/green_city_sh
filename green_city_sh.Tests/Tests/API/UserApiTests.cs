using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using Allure.Net.Commons.Attributes;
using NUnit.Framework;
using RestSharp;
using green_city_sh.Tests.Api.Clients;
using green_city_sh.Tests.Api.Clients.GreencityUser;
using green_city_sh.Tests.Api.DTO;
using green_city_sh.Tests.Api.DTO.User;
using green_city_sh.Tests.Infrastructure;

namespace green_city_sh.Tests.Tests.API;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
[AllureOwner("Nikita Muntianov")]
[AllureSubSuite("User-controller")]
public class UserApiTests : BaseAPITest
{
    private UserClient _userClient = null!;
    private UserClient _unauthorizedClient = null!;
    private OwnSecurityClient _securityClient = null!;

    private long _createdUserId;
    private string _createdUserEmail = string.Empty;
    private bool _isUserDeleted = false;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _securityClient = new OwnSecurityClient(Configuration.ApiUserBaseUrl);
        var signInModel = new SignInModal
        {
            email = Configuration.TestEmail,
            password = Configuration.TestPassword
        };

        RestResponse response = _securityClient.SignIn(signInModel);

        if (!response.IsSuccessful || string.IsNullOrEmpty(response.Content))
        {
            throw new Exception($"Auth failed. " +
                $"Status: {response.StatusCode}, " +
                $"Error: {response.ErrorMessage}, " +
                $"Exception: {response.ErrorException?.Message}");
        }

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var authResponse = JsonSerializer.Deserialize<AuthResponce>(response.Content, options);
        if (string.IsNullOrEmpty(authResponse?.accessToken))
        {
            throw new Exception("Access token is missing in the authentication response.");
        }

        // Initialize authorized client
        _userClient = new UserClient(Configuration.ApiUserBaseUrl, authResponse!.accessToken);

        // Initialize unauthorized client without a token
        _unauthorizedClient = new UserClient(Configuration.ApiUserBaseUrl);
    }

    [Test, Order(1)]
    [AllureDescription("Verify creating a user successfully")]
    [AllureTag("API", "Smoke")]
    public void VerifyCreateUserSuccessfully()
    {
        _createdUserId = Random.Shared.Next(1000000, 9999999);
        _createdUserEmail = $"autotest_{_createdUserId}@greencity.cx.ua";

        var requestDto = new CreateUserRequestDto
        {
            Id = _createdUserId,
            Email = _createdUserEmail,
            Name = "Automation Test User",
            ProfilePicturePath = null
        };

        var response = _userClient.CreateUser(requestDto);

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created), "Status code should be 201 Created");
            Assert.That(response.Content, Does.Contain("true"), "Response body should be true upon successful creation");
        });
    }

    [Test, Order(2)]
    [AllureDescription("Verify retrieving external user profiles successfully")]
    [AllureTag("API", "Smoke")]
    public void VerifyGetExternalProfilesSuccessfully()
    {
        var response = _userClient.GetExternalProfiles(new[] { _createdUserEmail });

        // Safe check before deserialization
        Assert.That(response.IsSuccessful, Is.True, $"Failed to get profiles. Status: {response.StatusCode}, Content: {response.Content}");

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var profiles = JsonSerializer.Deserialize<List<UserProfileExternalDto>>(response.Content!, options);

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Status code should be 200 OK");
            Assert.That(profiles, Is.Not.Null.And.Not.Empty, "Profiles list should not be empty");
            Assert.That(profiles![0].Email, Is.EqualTo(_createdUserEmail), "Returned profile email should match");
        });
    }

    [Test, Order(3)]
    [AllureDescription("Verify updating the user's name successfully")]
    [AllureTag("API", "Smoke")]
    public void VerifyUpdateUserNameSuccessfully()
    {
        string newUserName = "Updated Automation Name";

        var response = _userClient.UpdateUserName(_createdUserEmail, newUserName);

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Status code should be 200 OK after updating name");
        });
    }

    [Test, Order(4)]
    [AllureDescription("Verify updating the user's status successfully")]
    [AllureTag("API", "Smoke")]
    public void VerifyUpdateUserStatusSuccessfully()
    {
        string targetStatus = "ACTIVATED";

        var response = _userClient.UpdateUserStatus(_createdUserId, targetStatus);

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Status code should be 200 OK after updating status");
        });
    }

    [Test, Order(5)]
    [AllureDescription("Verify soft deleting a user successfully")]
    [AllureTag("API", "Smoke")]
    public void VerifySoftDeleteUser()
    {
        var response = _userClient.UpdateUserStatus(_createdUserId, "DELETED");

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Status code should be 200 OK after soft deleting user");
        });

        if (response.StatusCode == HttpStatusCode.OK)
        {
            _isUserDeleted = true;
        }
    }

    [Test, Order(6)]
    [AllureDescription("Verify deleting a user without authorization fails with 401")]
    [AllureTag("API", "Security")]
    public void VerifyDeleteUserUnauthorizedReturns401()
    {
        var response = _unauthorizedClient.DeleteUser();

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized), "Expected 401 Unauthorized when attempting to delete without a valid token");
        });
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        if (_createdUserId > 0 && !_isUserDeleted)
        {
            _userClient?.UpdateUserStatus(_createdUserId, "DELETED");
        }
    }
}
