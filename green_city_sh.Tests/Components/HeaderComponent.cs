using Allure.Net.Commons.Attributes;
using green_city_sh.Tests.Infrastructure;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace green_city_sh.Tests.Components;

public class HeaderComponent : BaseComponent
{
    private By HeaderLogo => By.CssSelector(".header_logo");
    private By NavigationLinks => By.CssSelector(".header_navigation-menu-left a");
    private By SearchBtn => By.CssSelector(".search-icon");
    private By LanguageDropdown => By.CssSelector(".header_lang-switcher-wrp");
    private By LanguageDropdownOptions => By.CssSelector(".header_lang-switcher-wrp li");
    private By SignInLink => By.XPath(".//a[contains(@class, 'header_sign-in-link')] | .//img[@alt='sing in button']");
    private By SignInModalRootLocator = By.CssSelector("app-auth-modal");
    private By SignUpLink => By.CssSelector(".header_sign-up-btn");
    private By BookmarkBtn => By.CssSelector(".bookmark-icon");
    private By NotificationsBtn => By.CssSelector(".notification-icon");
    private By UserProfileButton => By.CssSelector("#header_user-wrp");
    private By NotificationsOption => By.CssSelector("[aria-label='notifications']");
    private By CabinetOption => By.CssSelector("a[href*='/ubs/user/orders']");
    private By SignOutOption => By.CssSelector("[aria-label='sign-out']");

    private By UserName = By.CssSelector("#header_user-wrp .user-name");

    public HeaderComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public HeaderComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }

    public void ClickLogo()
    {
        var logo = RootElement.FindElement(HeaderLogo);
        logo.Click();
    }
    public void MapsTo(string tabName)
    {
        if (string.IsNullOrWhiteSpace(tabName))
            throw new ArgumentException("Tab name must be provided.", nameof(tabName));

        var allLinks = RootElement.FindElements(NavigationLinks);

        var targetLink = allLinks.FirstOrDefault(link =>
            link.Text.Contains(tabName, StringComparison.OrdinalIgnoreCase));

        if (targetLink != null)
            targetLink.Click();
        else
            throw new NoSuchElementException($"Tab with name '{tabName}' was not found");
    }

    public void OpenSearch()
    {
        var search = RootElement.FindElement(SearchBtn);
        search.Click();
    }

    public HeaderComponent ChangeLanguage(string langCode)
    {
        var dropdown = RootElement.FindElement(LanguageDropdown);

        dropdown.Click();

        var options = RootElement.FindElements(LanguageDropdownOptions);

        var targetOption = options.FirstOrDefault(link =>
            link.Text.Contains(langCode, StringComparison.OrdinalIgnoreCase));

        targetOption?.Click();

        return this;
    }

    public bool IsUserLoggedIn()
    {
        try
        {
            var hasSignIn = driver.FindElements(SignInLink).Count > 0;
            var hasProfile = driver.FindElements(UserProfileButton).Count > 0;
            return hasProfile && !hasSignIn;
        }
        catch (StaleElementReferenceException)
        {
            return false;
        }
    }

    public void WaitForUserLoggedIn()
    {
        new WebDriverWait(driver, TimeSpan.FromSeconds(Configuration.DefaultTimeout))
            .Until(_ => IsUserLoggedIn());
    }

    public SignInModalComponent ClickSignIn()
    {
        //var signInLink = RootElement.FindElement(SignInLink);
        var signInLink = wait.Until(d =>
        {
            try
            {
                var element = RootElement.FindElement(SignInLink);
                return element.Displayed ? element : null;
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
        });
        signInLink.Click();
        var signInModal = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(SignInModalRootLocator));
        return new SignInModalComponent(driver, signInModal);
    }
    public void ClickSignUp()
    {
        var signUpLink = RootElement.FindElement(SignUpLink);
        signUpLink.Click();
    }
    [AllureStep("Click the bookmarks button")]
    public void ClickBookmarks()
    {
        var bookmarksBtn = RootElement.FindElement(BookmarkBtn);
        bookmarksBtn.Click();
    }
    public void ClickNotifications()
    {
        var notificationsBtn = RootElement.FindElement(NotificationsBtn);
        notificationsBtn.Click();
    }
    public HeaderComponent UserProfileButtonClick()
    {
        var userProfileBtn = FindElement(UserProfileButton);
        userProfileBtn.Click();
        return this;
    }
    public void OpenNotificationsTab()
    {
        UserProfileButtonClick();
        var notificationsBtn = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(NotificationsOption));
        notificationsBtn.Click();
    }
    public void OpenPersonalCabinet()
    {
        UserProfileButtonClick();
        var cabinetBtn = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(CabinetOption));
        cabinetBtn.Click();
    }
    public bool WaitUntilUserLoggedIn()
    {
        return wait.Until(_ =>
        {
            var profileButtons = driver.FindElements(UserProfileButton);
            return profileButtons.Any(button => button.Displayed);
        });
    }

    public bool WaitUntilUserLoggedOut()
    {
        return wait.Until(_ =>
        {
            var signInLinks = driver.FindElements(SignInLink);
            return signInLinks.Any(link => link.Displayed);
        });
    }

    public void SignOut()
    {
        var profileButton = wait.Until(_ =>
        {
            var buttons = driver.FindElements(UserProfileButton);
            return buttons.FirstOrDefault(button => button.Displayed && button.Enabled);
        });

        profileButton!.Click();

        var signOutButton = wait.Until(_ =>
        {
            var buttons = driver.FindElements(SignOutOption);
            return buttons.FirstOrDefault(button => button.Displayed && button.Enabled);
        });

        signOutButton!.Click();
    }

    public IWebElement GetSignOutOption()
    {
        return FindElement(SignOutOption);
    }
}

