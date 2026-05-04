using NUnit.Framework;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;
using green_city_sh.Tests.Components;
using Allure.NUnit.Attributes;
using Allure.Net.Commons;

namespace green_city_sh.Tests.Tests;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class RegistrationTests : BaseTest
{
    private HomePage? _homePage;

    protected override void OnSetup()
    {
        _homePage = new HomePage(Driver!);
        _homePage.Open(BaseUrl);
        _homePage.Header.ChangeLanguage("En");
    }

    [Test]
    [Order(1)]
    [Category("Registration")]
    [Category("Smoke")]
    [Category("Regression")]
    [Description("TC-023: Successful registration of a new user with valid data")]
    [AllureFeature("Registration")]
    [AllureStory("TC-023: Successful registration of a new user with valid data")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureDescription("Verify that a new user can successfully register with valid data and receives a confirmation message.")]

    public void Register_WithValidData_ShouldShowSuccessMessage()
    {
        var email = $"greencitytest_{DateTime.Now:yyyyMMddHHmmss}@hotmail.com";
        const string userName = "Test";
        const string password = "ValidPass123!";

        _homePage!.Header.ClickSignUp();

        var signUpModal = SignUpModalComponent.WaitAndCreate(Driver!);

        signUpModal.FillRegistrationForm(email, userName, password);

        Assert.That(signUpModal.IsSignUpButtonEnabled(), Is.True,
            "Sign up button should be enabled for valid data.");

        signUpModal.ClickSignUp();

        var successMessage = signUpModal.GetSuccessMessage();

        Assert.Multiple(() =>
        {
            Assert.That(successMessage, Is.Not.Empty,
                "Success message should be displayed after registration.");

            Assert.That(successMessage,
                Is.EqualTo("Congratulations! You have successfully registered on the site. Please confirm your email address in the email box."),
                $"Unexpected success message: {successMessage}");
        });
    }

    [Test]
    [Category("Registration")]
    public void Register_WithInvalidEmail_ShouldShowValidationError()
    {
        const string invalidEmail = "invalidEmail";
        const string userName = "Test";
        const string password = "ValidPass123!";

        _homePage!.Header.ClickSignUp();

        var signUpModal = SignUpModalComponent.WaitAndCreate(Driver!);

        signUpModal.FillRegistrationForm(invalidEmail, userName, password);

        Assert.Multiple(() =>
        {
            Assert.That(signUpModal.IsInvalidEmailMessageDisplayed(), Is.True,
                "Validation message for invalid email should be displayed.");

            Assert.That(signUpModal.IsSignUpButtonEnabled(), Is.False,
                "Sign up button should be disabled for invalid email.");
        });
    }
}

