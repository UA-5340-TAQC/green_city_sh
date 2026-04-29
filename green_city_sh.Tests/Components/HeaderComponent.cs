using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class HeaderComponent : BaseComponent
{
    private By HeaderLogo => By.CssSelector(".header_logo");
    private By NavigationLinks => By.CssSelector(".header_navigation-menu-left a");
    private By SearchBtn => By.CssSelector(".search-icon");
    private By LanguageDropdown => By.CssSelector(".header_lang-switcher-wrp");
    private By LanguageDropdownOptions => By.CssSelector(".header_lang-switcher-wrp li");
    private By SignInLink => By.CssSelector(".header_sign-in-link");
    private By SignUpLink => By.CssSelector(".header_sign-up-btn");
    private By BookmarkBtn => By.CssSelector(".bookmark-icon");
    private By NotificationsBtn => By.CssSelector(".notification-icon");
    private By UserProfileButton => By.CssSelector("#header_user-wrp");
    private By NotificationsOption => By.CssSelector("[aria-label='notifications']");
    private By CabinetOption => By.CssSelector("a[href*='/ubs/user/orders']");
    private By SignOutOption => By.CssSelector("[aria-label='sign-out']");

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

    public void ChangeLanguage(string langCode)
    {
        var dropdown = RootElement.FindElement(LanguageDropdown);

        dropdown.Click();

        var options = RootElement.FindElements(LanguageDropdownOptions);

        var targetOption = options.FirstOrDefault(link =>
            link.Text.Contains(langCode, StringComparison.OrdinalIgnoreCase));

        targetOption?.Click();
    }

    public bool IsUserLoggedIn()
    {
        var hasSignIn = RootElement.FindElements(SignInLink).Count > 0;
        var hasProfile = RootElement.FindElements(UserProfileButton).Count > 0;
        return hasProfile && !hasSignIn;
    }
    public void ClickSignIn()
    {
        var signInLink = RootElement.FindElement(SignInLink);
        signInLink.Click();
    }
    public void ClickSignUp()
    {
        var signUpLink = RootElement.FindElement(SignUpLink);
        signUpLink.Click();
    }
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
    public void OpenNotificationsTab()
    {
        RootElement.FindElement(UserProfileButton).Click();
        var notificationsBtn = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(NotificationsOption));
        notificationsBtn.Click();
    }
    public void OpenPersonalCabinet()
    {
        RootElement.FindElement(UserProfileButton).Click();
        var cabinetBtn = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(CabinetOption));
        cabinetBtn.Click();
    }
    public void SignOut()
    {
        RootElement.FindElement(UserProfileButton).Click();
        var signOutBtn = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(SignOutOption));
        signOutBtn.Click();
    }
}
