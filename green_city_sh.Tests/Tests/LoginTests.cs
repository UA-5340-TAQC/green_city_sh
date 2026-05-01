using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;

namespace green_city_sh.Tests.Tests;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class LoginTests : BaseTest
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



        string accessToken = spacePage.GetAccessTokenFromLocalStorage();
        Assert.IsFalse(string.IsNullOrEmpty(accessToken), "Session token was not created in LocalStorage.");

        spacePage.WaitUntilPageLoads();
        Assert.IsTrue(Driver!.Url.Contains("/profile"), "User is not on the profile page.");

        spacePage.Header.UserProfileButtonClick();
        IWebElement signOutOption = spacePage.Header.GetSignOutOption();
        Assert.IsTrue(signOutOption.Displayed, "'Sign out' button is not visible.");
    }

    // Attempt to login with invalid email format
    [Test]
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
}