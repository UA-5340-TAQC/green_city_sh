using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class HeaderComponent: BaseComponent
{
    public HeaderComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public HeaderComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }
}
