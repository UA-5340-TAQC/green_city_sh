using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;
using NUnit.Framework;
using OpenQA.Selenium;

namespace green_city_sh.Tests.Tests;

[TestFixture]
public class LogoutTests : BaseTest
{
    private HomePage? _homePage;

    protected override void OnSetup()
    {
        _homePage = new HomePage(Driver!);
        _homePage.Open(BaseUrl);

        _homePage.Header.ChangeLanguage("En");
        _homePage.Header.ClickSignIn();

        var signInModal = SignInModalComponent.WaitAndCreate(Driver!);
        signInModal.Login(Configuration.TestEmail, Configuration.TestPassword);
    }

    [Test]
    [Category("Logout")]
    public void Logout_AfterSuccessfulLogin_ShouldEndUserSession()
    {
        const int userId = 1205;
        var protectedProfileUrl = $"{BaseUrl}/profile/{userId}";

        Assert.That(_homePage!.Header.WaitUntilUserLoggedIn(), Is.True,
            "User should be logged in before logout.");

        Driver!.Navigate().GoToUrl(protectedProfileUrl);

        Assert.That(Driver.Url, Does.Contain($"/profile/{userId}"),
            "User should be able to access profile page before logout.");

        _homePage.Header.SignOut();

        Assert.Multiple(() =>
        {
            Assert.That(_homePage.Header.WaitUntilUserLoggedOut(), Is.True,
                "User should not be logged in after logout.");

            Assert.That(Driver!.Url, Does.Contain("greenCity"),
                "User should be redirected to the main page after logout.");

            Assert.That(IsAuthTokenPresent(), Is.False,
                "Auth token should be removed after logout.");
        });

        Driver.Navigate().Refresh();
        Driver.Navigate().Back();

        Driver.Navigate().GoToUrl(protectedProfileUrl);

        Assert.That(_homePage.Header.WaitUntilUserLoggedOut(), Is.True,
            "User should not be able to access protected resources after logout.");
    }

    private bool IsAuthTokenPresent()
    {
        var token = ((IJavaScriptExecutor)Driver!).ExecuteScript(@"
            return window.localStorage.getItem('accessToken') 
                || window.localStorage.getItem('token') 
                || window.sessionStorage.getItem('accessToken') 
                || window.sessionStorage.getItem('token');
        ");

        return token != null;
    }
}
