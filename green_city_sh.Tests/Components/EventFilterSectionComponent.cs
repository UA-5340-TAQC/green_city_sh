using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class EventFilterSectionComponent : BaseComponent
{
    public EventFilterSectionComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public EventFilterSectionComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }
}