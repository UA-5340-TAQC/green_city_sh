using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class FooterComponent : BaseComponent
{
    public FooterComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public FooterComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }
}
