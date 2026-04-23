using NUnit.Framework;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;
using green_city_sh.Tests.Components;

namespace green_city_sh.Tests.Tests;

[TestFixture]
public class RegistrationTests : BaseTest
{
    private HomePage? _homePage;

    protected override void OnSetup()
    {
        _homePage = new HomePage(Driver!);
        _homePage.Open(BaseUrl);
    }

    [Test]
    [Category("Registration")]
    public void Register_WithValidData_ShouldShowSuccessMessage()
    {
        // Arrange
        string email = $"greencitytest_{DateTime.Now:yyyyMMddHHmmss}@hotmail.com";
        const string userName = "Test";
        const string password = "ValidPass123!";

        // Act
        _homePage!.Header.ClickSignUp();
        SignUpModalComponent signUpModal = SignUpModalComponent.WaitAndCreate(Driver!);

        signUpModal.EnterEmail(email);
        signUpModal.EnterUserName(userName);
        signUpModal.EnterPassword(password);
        signUpModal.EnterConfirmPassword(password);

        Assert.That(signUpModal.IsSignUpButtonEnabled(), Is.True,
            "Sign up button should be enabled for valid data.");

        signUpModal.ClickSignUp();

        string successMessage = signUpModal.GetSuccessMessage();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(successMessage, Is.Not.Empty,
                "Success message should be displayed after registration.");

            Assert.That(successMessage,
                Is.AnyOf(
                    "Congratulations! You have successfully registered on the site. Please confirm your email address in the email box.",
                    "Вітання! Ви успішно зареєструвалися на сайті. Будь ласка, підтвердіть свою адресу електронної пошти."
                ),
                $"Unexpected success message: {successMessage}");
        });
    }

    [Test]
    [Category("Registration")]
    public void Register_WithInvalidEmail_ShouldShowValidationError()
    {
        // Arrange
        const string invalidEmail = "invalidEmail";
        const string userName = "Test";
        const string password = "ValidPass123!";

        // Act
        _homePage!.Header.ClickSignUp();
        SignUpModalComponent signUpModal = SignUpModalComponent.WaitAndCreate(Driver!);

        signUpModal.EnterEmail(invalidEmail);
        signUpModal.EnterUserName(userName);
        signUpModal.EnterPassword(password);
        signUpModal.EnterConfirmPassword(password);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(signUpModal.IsInvalidEmailMessageDisplayed(), Is.True,
                "Validation message for invalid email should be displayed.");

            Assert.That(signUpModal.IsSignUpButtonEnabled(), Is.False,
                "Sign up button should be disabled for invalid email.");
        });
    }

}