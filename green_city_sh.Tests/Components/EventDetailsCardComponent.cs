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
}