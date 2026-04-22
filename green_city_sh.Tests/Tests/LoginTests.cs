using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;
using OpenQA.Selenium;

namespace green_city_sh.Tests.Tests;

[TestFixture]
public class LoginTests : BaseTest
{
    private HomePage homePage;
    private HeaderComponent header;
    private SignInModalComponent signInModal;

    [SetUp]
    public void SetupTest()
    {
        NavigateToBaseUrl();
        homePage = new HomePage(Driver);
        header = new HeaderComponent(Driver, By.CssSelector("header[role='banner']"));
    }

    [Test]
    [Category("Smoke")]
    [Category("Regression")]
    public void AttemptToLoginWithInvalidEmail()
    {
        string invalidEmail = "invalidemail@invalid";
        string password = "asweQA5346!)";
        string expectedErrorMessage = "Please check that your e-mail address is indicated correctly";

        header.ChangeLanguage("En");

        header.ClickSignIn();

        signInModal = SignInModalComponent.WaitAndCreate(Driver);

        Assert.That(signInModal.IsModalVisible(), Is.True, "Sign in modal should be visible");

        signInModal.EnterEmail(invalidEmail);

        signInModal.EnterPassword(password);

        string errorMessage = signInModal.GetEmailErrorMessage();

        Assert.IsTrue(errorMessage.Contains(expectedErrorMessage));

        Assert.That(signInModal.IsSignInButtonEnabled(), Is.False,
            "Sign In button should be disabled when email format is invalid");

        signInModal.CloseModal();
        Assert.That(header.IsUserLoggedIn(), Is.False,
            "User should not be logged in after attempting with invalid email");
    }
}
