using green_city_sh.Tests.Components;
 using green_city_sh.Tests.Infrastructure;
 using OpenQA.Selenium;
 using OpenQA.Selenium.Interactions;
 using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace green_city_sh.Tests.Pages;

public class EventDetailsPage : BasePage
{ 
    private const string EventsListUrl = "/events";
    
    private static readonly By FirstEventCardLocator =
        By.CssSelector("app-events-list-item");
    
    private static readonly By PageReadyLocator =
        By.CssSelector("app-event-details, app-add-comment, div.comments-title");

    private static readonly By MoreButtonLocator = By.XPath(
        "//button[contains(normalize-space(.), 'More')]");
    
    public EventDetailsPage(IWebDriver driver) : base(driver)
    { }

    public EventDetailsPage Open()
    {
        driver.Navigate().GoToUrl($"{Configuration.BaseUrl}{EventsListUrl}");
        WaitUntilPageLoads();
        
        var firstCard = new WebDriverWait(driver, 
            TimeSpan.FromSeconds(Configuration.DefaultTimeout))
            .Until(drv =>
                drv.FindElements(FirstEventCardLocator)
                    .FirstOrDefault(el => el.Displayed))
            ?? throw new WebDriverTimeoutException(
                "No visible event details button was found on the events list page.");
        
        new Actions(driver)
            .ScrollToElement(firstCard)
            .Perform();
        
        firstCard.FindElement(MoreButtonLocator).Click();
        new WebDriverWait(driver,
                TimeSpan.FromSeconds(Configuration.DefaultTimeout))
            .Until(drv =>
                drv.Url.Contains("/events/", StringComparison.OrdinalIgnoreCase)
                && !drv.Url.EndsWith("/events/")
                && !drv.Url.EndsWith("/events"));

        WaitUntilPageLoads();

        return this;
    }
    
    public EventDetailsPage OpenViaMoreButton()
    {
        driver.Navigate().GoToUrl($"{Configuration.BaseUrl}/events");
        WaitUntilPageLoads();

        var moreButton = wait.Until(drv =>
                             drv.FindElements(MoreButtonLocator)
                                 .FirstOrDefault(button => button.Displayed && button.Enabled))
                         ?? throw new WebDriverTimeoutException("No visible event details button was found on the events page.");

        moreButton.Click();
        wait.Until(drv => drv.Url.Contains("/events/", StringComparison.OrdinalIgnoreCase));
        WaitUntilPageLoads();

        return this;
    }
    
    public EventDetailsPage RefreshPage()
    {
        driver.Navigate().Refresh();
        return this;
        driver.Navigate().GoToUrl($"{Configuration.BaseUrl}/events");
        WaitUntilPageLoads();

        var moreButton = wait.Until(drv =>
                             drv.FindElements(MoreButtonLocator)
                                 .FirstOrDefault(button => button.Displayed && button.Enabled))
                         ?? throw new WebDriverTimeoutException("No visible event details button was found on the events page.");

        moreButton.Click();
        wait.Until(drv => drv.Url.Contains("/events/", StringComparison.OrdinalIgnoreCase));
        WaitUntilPageLoads();

        return this;
    }

    public CommentsComponent GetCommentsComponent()
        => CommentsComponent.WaitAndCreate(driver);
    
}
