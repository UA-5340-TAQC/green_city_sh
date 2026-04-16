using OpenQA.Selenium;
using green_city_sh.Tests.Infrastructure;

namespace green_city_sh.Tests.Components;

public abstract class TagsComponent : Base
{
    protected IWebElement RootElement;
    protected TagsComponent(IWebDriver driver, By rootLocator) : base(driver)
    {
        RootElement = driver.FindElement(rootLocator);
    }
    protected TagsComponent(IWebDriver driver, IWebElement componentRoot) : base(driver)
    {
        RootElement = componentRoot;
    }
}
