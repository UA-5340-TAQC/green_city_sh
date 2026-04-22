using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class EventInfoComponent: BaseComponent
{
    public EventInfoComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public EventInfoComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }

    private By EventTitle => By.CssSelector(".event-title"); 

    private By DateOnly => By.CssSelector("div.event-date-content > div:nth-child(1)");

    private By TimeOnly => By.CssSelector("div.event-date-content > div:nth-child(3)");

    private By EventLocation => By.CssSelector("div.event-location"); 

    private By EventAuthor => By.CssSelector("div.event-author");

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