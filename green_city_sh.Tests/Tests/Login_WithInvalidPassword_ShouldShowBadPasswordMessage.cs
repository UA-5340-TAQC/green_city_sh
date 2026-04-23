using NUnit.Framework;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;
using green_city_sh.Tests.Components;

namespace green_city_sh.Tests.Tests;

[TestFixture]
public class InvalidPasswordTests : BaseTest
{
    private HomePage? _homePage;

    protected override void OnSetup()
    {
        _homePage = new HomePage(Driver!);
        _homePage.Open(BaseUrl);
    }

    [Test]
    [Category("Login")]
    public void Login_WithInvalidPassword_ShouldShowError()
    {
        // Arrange
        const string email = "greencitytest69@hotmail.com";
        const string invalidPassword = "wrongPassword123!";

        // Act
        _homePage!.Header.ClickSignIn();
        SignInModalComponent signInModal = SignInModalComponent.WaitAndCreate(Driver!);

        signInModal.EnterEmail(email);
        signInModal.EnterPassword(invalidPassword);
        signInModal.ClickSignIn();

        var actualErrorMessage = signInModal.GetErrorMessage();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(signInModal.IsModalVisible(), Is.True,
                "User should remain on the sign-in modal.");

            Assert.That(actualErrorMessage,
                Is.AnyOf("Bad password", "Неправильний пароль"),
                $"Unexpected error message: {actualErrorMessage}");

            Assert.That(_homePage.Header.IsUserLoggedIn(), Is.False,
                "User should not be logged in.");
        });
    }
}