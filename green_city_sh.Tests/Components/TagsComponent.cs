using OpenQA.Selenium;
using green_city_sh.Tests.Infrastructure;

namespace green_city_sh.Tests.Components;

public abstract class TagsComponent : BaseComponent
{
    protected TagsComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    protected TagsComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }
}
