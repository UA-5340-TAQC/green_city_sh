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
}