using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class NewsDetailsInfoComponent : BaseComponent
{
    public NewsDetailsInfoComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public NewsDetailsInfoComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }
}
