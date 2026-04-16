using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class EventsHeaderComponent : BaseComponent
{
    public EventsHeaderComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public EventsHeaderComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }
}