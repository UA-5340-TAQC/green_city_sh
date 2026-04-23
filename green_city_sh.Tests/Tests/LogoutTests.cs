using NUnit.Framework;using OpenQA.Selenium;using OpenQA.Selenium.Support.UI;using green_city_sh.Tests.Infrastructure;using green_city_sh.Tests.Pages;using green_city_sh.Tests.Components;
namespace green_city_sh.Tests.Tests;
[TestFixture]public class LogoutTests : BaseTest{    private HomePage? _homePage;
    private readonly By UserProfileButton = By.CssSelector("#header_user-wrp");    private readonly By SignOutOption = By.CssSelector("[aria-label='sign-out']");    private readonly By SignInLink = By.CssSelector(".header_sign-in-link");
    protected override void OnSetup()    {        _homePage = new HomePage(Driver!);        _homePage.Open(BaseUrl);    }
    [Test]    [Category("Logout")]    public void Logout_AfterSuccessfulLogin_ShouldEndUserSession()    {        const string email = "greencitytest69@hotmail.com";        const string password = "asweQA5346!)";
        _homePage!.Header.ClickSignIn();
        SignInModalComponent signInModal = SignInModalComponent.WaitAndCreate(Driver!);        signInModal.Login(email, password);
        Assert.That(WaitUntilUserLoggedIn(), Is.True,
                    "User should be logged in before logout.");
        ClickSignOut();
        Assert.Multiple(() =>        {            Assert.That(WaitUntilUserLoggedOut(), Is.True,                "User should not be logged in after logout.");
            Assert.That(Driver!.Url, Does.Contain("greenCity"),
                            "User should be on the main page after logout.");
            Assert.That(IsAuthTokenPresent(), Is.False,
                            "Auth token should be removed after logout.");        });    }
    private void ClickSignOut()    {        var wait = new WebDriverWait(            Driver!,            TimeSpan.FromSeconds(Configuration.DefaultTimeout));
        var profileButton = wait.Until(driver =>        {            var buttons = driver.FindElements(UserProfileButton);            return buttons.FirstOrDefault(button => button.Displayed && button.Enabled);        });
        profileButton!.Click();
        var signOutButton = wait.Until(driver =>        {            var buttons = driver.FindElements(SignOutOption);            return buttons.FirstOrDefault(button => button.Displayed && button.Enabled);        });
        signOutButton!.Click();    }
    private bool WaitUntilUserLoggedIn()    {        var wait = new WebDriverWait(            Driver!,            TimeSpan.FromSeconds(Configuration.DefaultTimeout));
        return wait.Until(driver =>        {            var profileButtons = driver.FindElements(UserProfileButton);            return profileButtons.Any(button => button.Displayed);        });    }
    private bool WaitUntilUserLoggedOut()    {        var wait = new WebDriverWait(            Driver!,            TimeSpan.FromSeconds(Configuration.DefaultTimeout));
        return wait.Until(driver =>        {            var signInLinks = driver.FindElements(SignInLink);            return signInLinks.Any(link => link.Displayed);        });    }
    private bool IsAuthTokenPresent()    {        var token = ((IJavaScriptExecutor)Driver!).ExecuteScript(@"            return window.localStorage.getItem('accessToken')                 || window.localStorage.getItem('token')                 || window.sessionStorage.getItem('accessToken')                 || window.sessionStorage.getItem('token');        ");
        return token != null;    }}
