using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class EventListComponent : BaseComponent
{
    public EventListComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public EventListComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }
}