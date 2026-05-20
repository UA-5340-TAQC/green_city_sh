using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using Allure.Net.Commons.Attributes;
using NUnit.Framework;
using RestSharp;
using green_city_sh.Tests.Api.Clients.GreencityUser;
using green_city_sh.Tests.Api.DTO;
using green_city_sh.Tests.Api.DTO.User;
using green_city_sh.Tests.Infrastructure;

namespace green_city_sh.Tests.Tests.API;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
[AllureOwner("Nikita Muntianov")]
[AllureSubSuite("User-Lifecycle-Controller")]
public class UserLifecycleApiTests : BaseAPITest
{
    private OwnSecurityClient _securityClient = null!;
    private UserClient _userClient = null!;

    // State Tracking
    private string _dynamicEmail = string.Empty;
    private string _dynamicPassword = string.Empty;
    private string _dynamicName = string.Empty;
    private string _accessToken = string.Empty;
    private bool _isDeleted = false;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _securityClient = new OwnSecurityClient(Configuration.ApiUserBaseUrl);
        string uniqueId = Guid.NewGuid().ToString("N")[..8];

        _dynamicName = "AutoUser";
        _dynamicEmail = $"qa.auto.{uniqueId}@greencity.cx.ua";

        _dynamicPassword = $"Pass1!{uniqueId}";
    }

    [Test, Order(1)]
    [AllureDescription("Verify user can successfully sign up")]
    [AllureTag("API", "Functional", "Lifecycle")]
    public void VerifyUserRegistration_Returns201()
    {
        var signUpRequest = new SignUpRequestDto
        {
            Name = _dynamicName,
            Email = _dynamicEmail,
            Password = _dynamicPassword,
            IsUbs = true
        };

        var response = _securityClient.SignUp(signUpRequest);

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created),
                $"Status code should be 201 Created. Server response: {response.Content}");
            Assert.That(response.Content, Is.Not.Null.And.Not.Empty, "Response content should not be empty");
        });
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var signUpResponse = JsonSerializer.Deserialize<SignUpResponseDto>(response.Content!, options);

        Assert.That(signUpResponse?.Email, Is.EqualTo(_dynamicEmail), "Email in response should match the registered email");
    }

    [Test, Order(2)]
    [AllureDescription("Verify user can successfully sign in after registration")]
    [AllureTag("API", "Functional", "Lifecycle")]
    public void VerifyUserLogin_Returns200AndToken()
    {
        var signInModel = new SignInModal
        {
            email = _dynamicEmail,
            password = _dynamicPassword,
            projectName = "GREENCITY"
        };

        var response = _securityClient.SignIn(signInModel);

        // NOTE: If this fails with 400 Bad Request, it means the system requires Email Verification (clicking a link in an email) before login is allowed.
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Status code should be 200 OK for successful login");
            Assert.That(response.Content, Is.Not.Null.And.Not.Empty, "Response content should not be empty");
        });

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var authResponse = JsonSerializer.Deserialize<AuthResponce>(response.Content!, options);

        Assert.That(authResponse?.accessToken, Is.Not.Null.And.Not.Empty, "Access token should be present in response");

        _accessToken = authResponse!.accessToken;
        _userClient = new UserClient(Configuration.ApiUserBaseUrl, _accessToken);
    }

    [Test, Order(3)]
    [AllureDescription("Verify user can successfully update their own profile")]
    [AllureTag("API", "Functional", "Lifecycle")]
    public void VerifyUserProfileUpdate_Returns200()
    {
        Assert.That(string.IsNullOrEmpty(_accessToken), Is.False, "Precondition failed: Access token is missing.");

        string updatedName = $"{_dynamicName}_Updated";

        var updateRequest = new ProfileUpdateRequestDto
        {
            Name = updatedName,
            ShowLocation = "PUBLIC",
            ShowEcoPlace = "PUBLIC",
            ShowToDoList = "PUBLIC",
            SocialNetworks = new List<string>(),
            Coordinates = new CoordinatesDto { Latitude = 50.4501, Longitude = 30.5234 }, // Example: Kyiv
            EmailPreferences = new List<EmailPreferenceDto>
            {
                new EmailPreferenceDto { EmailPreference = "SYSTEM", Periodicity = "IMMEDIATELY" }
            }
        };

        var response = _userClient.UpdateUserProfile(updateRequest);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Status code should be 200 OK after profile update");
    }

    [Test, Order(4)]
    [AllureDescription("Verify user can successfully delete their own account")]
    [AllureTag("API", "Functional", "Lifecycle")]
    public void VerifySelfDeletion_Returns200()
    {
        Assert.That(string.IsNullOrEmpty(_accessToken), Is.False, "Precondition failed: Access token is missing.");

        var response = _userClient.DeleteUser();

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Status code should be 200 OK after account deletion");

        if (response.StatusCode == HttpStatusCode.OK)
        {
            _isDeleted = true;
        }
    }

    [Test, Order(5)]
    [AllureDescription("Verify login fails after account is deleted")]
    [AllureTag("API", "Security", "Lifecycle")]
    public void VerifyLoginFails_AfterAccountDeletion_Returns400()
    {
        var signInModel = new SignInModal
        {
            email = _dynamicEmail,
            password = _dynamicPassword,
            projectName = "GREENCITY"
        };

        var response = _securityClient.SignIn(signInModel);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest).Or.EqualTo(HttpStatusCode.NotFound),
            "Status code should be 400 or 404 when trying to login with a deleted account");
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        // Fail-safe: Clean up the user if the test failed midway (e.g., failed on Step 3)
        if (!string.IsNullOrEmpty(_accessToken) && !_isDeleted)
        {
            try
            {
                _userClient?.DeleteUser();
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"Fail-safe deletion failed during teardown: {ex.Message}");
            }
        }
    }
}
