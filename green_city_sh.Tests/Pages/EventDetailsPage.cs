﻿﻿using green_city_sh.Tests.Components;
 using green_city_sh.Tests.Infrastructure;
 using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace green_city_sh.Tests.Pages;

public class EventDetailsPage : BasePage
{ 
    private const string EventUrl = "/events/36";
    
    private static readonly By PageReadyLocator =
        By.CssSelector("app-event-details, app-add-comment, div.comments-title");

    private static readonly By MoreButtonLocator = By.XPath(
        "//button[contains(normalize-space(.), 'More') or contains(normalize-space(.), 'Більше')]");
    
    public EventDetailsPage(IWebDriver driver) : base(driver)
    { }

    public EventDetailsPage Open()
    {
        driver.Navigate().GoToUrl($"{Configuration.BaseUrl}{EventUrl}");
        wait.Until(drv =>
            drv.Url.Contains("events/36", StringComparison.OrdinalIgnoreCase));

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

    public CommentsComponent GetCommentsComponent()
        => CommentsComponent.WaitAndCreate(driver);
    
}
