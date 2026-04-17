using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class EventHeaderComponent : BaseComponent
{
    public EventHeaderComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public EventHeaderComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }
}