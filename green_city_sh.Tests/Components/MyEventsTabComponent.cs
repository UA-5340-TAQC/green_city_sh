using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class MyEventsTabComponent : BaseComponent
{
    public MyEventsTabComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public MyEventsTabComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }
}
