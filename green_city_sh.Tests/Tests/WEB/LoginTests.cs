using OpenQA.Selenium;
using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;
using Allure.NUnit.Attributes;

namespace green_city_sh.Tests.Tests.WEB;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
[TestFixture]
[AllureSubSuite("LoginPage")]
[AllureTag("Smoke", "Regression")]
[AllureFeature("Authentication")]
public class LoginTests : BaseUITest
{
    private HomePage homePage = null!;

    protected override void OnSetup()
    {
        NavigateToBaseUrl();
        homePage = new HomePage(Driver!);
    }

    // Successful login with valid credentials
    [Test]
    [Order(1)]
    [Description("TC-024: Successful login to the system with valid credentials")]
    [Category("Smoke")]
    public void VerifySuccessfulLoginWithValidCredentials()
    {

        homePage.Header.ClickSignIn();
        SignInModalComponent signInModal = SignInModalComponent.WaitAndCreate(Driver!);

        signInModal.Login(Configuration.TestEmail, Configuration.TestPassword);
        MySpacePage spacePage = new MySpacePage(Driver!);

        string? accessToken = spacePage.GetAccessTokenFromLocalStorage();
        Assert.IsFalse(string.IsNullOrEmpty(accessToken), "Session token was not created in LocalStorage.");

        spacePage.WaitUntilPageLoads();
        Assert.IsTrue(Driver!.Url.Contains("/profile"), "User is not on the profile page.");

        spacePage.Header.UserProfileButtonClick();
        IWebElement signOutOption = spacePage.Header.GetSignOutOption();
        Assert.IsTrue(signOutOption.Displayed, "'Sign out' button is not visible.");
    }

    // Attempt to login with invalid email format
    [Test]
    [AllureIssue("26")]
    [AllureTag("NegativeTest", "Login")]
    [AllureFeature("Invalid Email Validation")]
    [AllureDescription("Verify that login with invalid email format shows error and disables Sign In button")]
    [Category("Smoke")]
    [Category("Regression")]
    public void AttemptToLoginWithInvalidEmail()
    {
        string invalidEmail = "invalid-email";
        string expectedErrorMessage = "Please check that your e-mail address is indicated correctly";

        homePage.Header.ChangeLanguage("En");
        homePage.Header.ClickSignIn();

        SignInModalComponent signInModal = SignInModalComponent.WaitAndCreate(Driver!);
        Assert.That(signInModal.IsModalVisible(), Is.True, "Sign in modal should be visible");

        signInModal.EnterEmail(invalidEmail);
        signInModal.EnterPassword(Configuration.TestPassword);

        string errorMessage = signInModal.GetEmailErrorMessage();

        Assert.That(errorMessage, Does.Contain(expectedErrorMessage),
            $"Expected email validation message to contain '{expectedErrorMessage}', but was '{errorMessage}'.");

        Assert.That(signInModal.IsSignInButtonEnabled(), Is.False,
            "Sign In button should be disabled when email format is invalid");

        signInModal.CloseModal();
        Assert.That(homePage.Header.IsUserLoggedIn(), Is.False,
            "User should not be logged in after attempting with invalid email");
    }

    // Attempt to login with invalid password
    [Test]
    [Order(3)]
    [Description("TC-025: Attempt to login with invalid password")]
    [Category("Smoke")]
    [Category("Regression")]
    public void AttemptToLoginWithInvalidPassword()
    {
        string invalidPassword = "WrongPassword123!";
        string expectedErrorMessage = "Bad password";

        homePage.Header.ChangeLanguage("En");
        homePage.Header.ClickSignIn();

        SignInModalComponent signInModal = SignInModalComponent.WaitAndCreate(Driver!);

        signInModal.EnterEmail(Configuration.TestEmail);
        signInModal.EnterPassword(invalidPassword);
        signInModal.ClickSignIn();

        string actualErrorMessage = signInModal.GetErrorMessage();

        Assert.That(actualErrorMessage, Does.Contain(expectedErrorMessage),
            $"Expected error message to contain '{expectedErrorMessage}', but was '{actualErrorMessage}'.");

        Assert.That(signInModal.IsModalVisible(), Is.True,
            "Sign in modal should remain visible after invalid password.");

        Assert.That(homePage.Header.IsUserLoggedIn(), Is.False,
            "User should not be logged in after attempting with invalid password.");

    }
}
