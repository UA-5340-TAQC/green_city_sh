using OpenQA.Selenium;
using green_city_sh.Tests.Infrastructure;

namespace green_city_sh.Tests.Components;

public abstract class NewsFormComponent : Base
{
    protected IWebElement RootElement;
    protected NewsFormComponent(IWebDriver driver, By rootLocator) : base(driver)
    {
        RootElement = driver.FindElement(rootLocator);
    }
    protected NewsFormComponent(IWebDriver driver, IWebElement componentRoot) : base(driver)
    {
        RootElement = componentRoot;
    }
}
