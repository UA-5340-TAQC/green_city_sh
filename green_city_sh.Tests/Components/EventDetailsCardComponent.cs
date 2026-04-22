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

    public bool IsEventSaved()
    {
        var btn = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(SaveEventButton));
        string btnText = btn.Text.Trim().ToLower();

        return btnText.Contains("unsave") || btnText.Contains("відмінити");
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
}