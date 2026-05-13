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

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var authResponse = JsonSerializer.Deserialize<AuthResponce>(response.Content!, options);

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
        _createdUserId = new Random().Next(1000000, 9999999);
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

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var profiles = JsonSerializer.Deserialize<List<UserProfileExternalDto>>(response.Content!, options);

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Status code should be 200 OK");
            Assert.That(profiles, Is.Not.Null.And.Not.Empty, "Profiles list should not be empty");
        });

        if (profiles != null && profiles.Count > 0)
        {
            Assert.That(profiles[0].Email, Is.EqualTo(_createdUserEmail), "Returned profile email should match the requested email");
        }
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
    [AllureDescription("Verify deleting a user without authorization fails with 401")]
    [AllureTag("API", "Security")]
    public void VerifyDeleteUserUnauthorizedReturns401()
    {
        // Strictly using the unauthorized client to satisfy the DELETE negative test constraint
        var response = _unauthorizedClient.DeleteUser();

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized), "Expected 401 Unauthorized when attempting to delete without a valid token");
        });
    }
}
