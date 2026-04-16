using OpenQA.Selenium;
using green_city_sh.Tests.Infrastructure;

namespace green_city_sh.Tests.Components;

public abstract class BaseComponent : Base
{
    protected IWebElement RootElement;
    protected BaseComponent(IWebDriver driver, By rootLocator) : base(driver)
    {
        RootElement = driver.FindElement(rootLocator);
    }
    protected BaseComponent(IWebDriver driver, IWebElement componentRoot) : base(driver)
    {
        RootElement = componentRoot;
    }
}
