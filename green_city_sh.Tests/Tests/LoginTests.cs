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
    private HeaderComponent header = null!;
    private SignInModalComponent signInModal = null!;

    protected override void OnSetup()
    {
        NavigateToBaseUrl(); 
        homePage = new HomePage(Driver!);
        header = homePage.Header;
    }

    // Successful login with valid credentials
    [Test]
    [Order(1)]
    [Description("TC-024: Successful login to the system with valid credentials")]
    [Category("Smoke")]
    public void VerifySuccessfulLoginWithValidCredentials()
    {
        var wait = new WebDriverWait(Driver!, TimeSpan.FromSeconds(Configuration.DefaultTimeout));

        header.ClickSignIn();
        signInModal = SignInModalComponent.WaitAndCreate(Driver!);

        signInModal.Login("greencitytest69@hotmail.com", "asweQA5346!)");
        
        var userProfileBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#header_user-wrp")));

        IJavaScriptExecutor js = (IJavaScriptExecutor)Driver!;
        string? accessToken = (string?)js.ExecuteScript("return window.localStorage.getItem('accessToken');");
        Assert.IsFalse(string.IsNullOrEmpty(accessToken), "Session token was not created in LocalStorage.");

        wait.Until(d => d.Url.Contains("/profile"));
        Assert.IsTrue(Driver!.Url.Contains("/profile"), "User is not on the profile page.");

        userProfileBtn.Click();
        var signOutOption = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[aria-label='sign-out']")));
        Assert.IsTrue(signOutOption.Displayed, "'Sign out' button is not visible.");
    }

    // Attempt to login with invalid email format
    [Test]
    [Category("Smoke")]
    [Category("Regression")]
    public void AttemptToLoginWithInvalidEmail()
    {
        string invalidEmail = "invalid-email";
        string password = "asweQA5346!)";
        string expectedErrorMessage = "Please check that your e-mail address is indicated correctly";

        header.ChangeLanguage("En");
        header.ClickSignIn();

        signInModal = SignInModalComponent.WaitAndCreate(Driver!);

        Assert.That(signInModal.IsModalVisible(), Is.True, "Sign in modal should be visible");

        signInModal.EnterEmail(invalidEmail);
        signInModal.EnterPassword(password);

        string errorMessage = signInModal.GetEmailErrorMessage();

        Assert.That(errorMessage, Does.Contain(expectedErrorMessage),
            $"Expected email validation message to contain '{expectedErrorMessage}', but was '{errorMessage}'.");

        Assert.That(signInModal.IsSignInButtonEnabled(), Is.False,
            "Sign In button should be disabled when email format is invalid");

        signInModal.CloseModal();
        Assert.That(header.IsUserLoggedIn(), Is.False,
            "User should not be logged in after attempting with invalid email");
    }
}