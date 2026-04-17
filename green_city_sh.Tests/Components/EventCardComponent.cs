using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class EventCardComponent : BaseComponent
{
    public EventCardComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public EventCardComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }
}