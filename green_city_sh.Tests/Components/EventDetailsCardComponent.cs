using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class EventDetailsCardComponent : BaseComponent
{
    public EventDetailsCardComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public EventDetailsCardComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }

    private By SaveEventButton => By.CssSelector("div.save-join-event-block button.secondary-global-button:nth-of-type(1)");
    private By CancelRequestButton => By.CssSelector("div.save-join-event-block button.secondary-global-button:nth-of-type(2)");
    private By JoinEventButton => By.CssSelector("div.save-join-event-block button.primary-global-button");
    private By BackToEventsButton => By.XPath(".//div[@class='button-text']");
    private By EventTitle => By.CssSelector(".event-title"); 
    private By DateOnly => By.CssSelector("div.event-date-content > div:nth-child(1)");
    private By TimeOnly => By.CssSelector("div.event-date-content > div:nth-child(3)");
    private By EventLocation => By.CssSelector("div.event-location"); 
    private By EventAuthor => By.CssSelector("div.event-author");

    public bool IsEventSaved()
    {
        var btn = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(SaveEventButton));
        string btnText = btn.Text.Trim().ToLower();

        return btnText.Contains("unsave") || btnText.Contains("відмінити");
    }

    public bool IsJoinRequestSent()
    {
            var cancelBtn = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(CancelRequestButton));
            return cancelBtn.Displayed;
    }
    public bool IsJoinEventButtonVisible()
    {
            var joinBtn = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(JoinEventButton));
            return joinBtn.Displayed;
    }
    public bool IsJoinRequestCancelled()
    {
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(CancelRequestButton));
    }
    public void ClickSaveEvent()
    {
        if (!IsEventSaved())
        {
            var saveButton = RootElement.FindElement(SaveEventButton);
            saveButton.Click();
        }
    }

    public void ClickUnsaveEvent()
    {
        if (IsEventSaved())
        {
            var saveButton = RootElement.FindElement(SaveEventButton);
            saveButton.Click();
        }
    }

    public void ClickJoinEvent()
    {
        var joinButton = RootElement.FindElement(JoinEventButton);
        joinButton.Click();
    }    

    public void ClickCancelRequest()
    {
        var cancelButton = RootElement.FindElement(CancelRequestButton);
        cancelButton.Click();
    }

    public void ClickBackToEvents()
    {
        var backButton = RootElement.FindElement(BackToEventsButton);
        
        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
        js.ExecuteScript("arguments[0].click();", backButton);
    }

    public string GetEventTitle()
    {
        return RootElement.FindElement(EventTitle).Text.Trim();
    }

    public string GetEventDate()
    {
        string date = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(DateOnly)).Text.Trim();
        string time = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(TimeOnly)).Text.Trim();

        return $"{date} | {time}"; 
    }

    public string GetEventLocation()
    {
        return RootElement.FindElement(EventLocation).Text.Trim();
    }

    public string GetEventAuthor()
    {
        return RootElement.FindElement(EventAuthor).Text.Trim();
    }

    public string GetEventInfo()
    {
        string title = GetEventTitle();
        string date = GetEventDate();
        //string location = GetEventLocation();
        string author = GetEventAuthor();

        return $"Title: {title}\nDate: {date}\nAuthor: {author}";
    }
}