using OpenQA.Selenium;
using green_city_sh.Tests.Infrastructure;

namespace green_city_sh.Tests.Components;

public abstract class NewsFormComponent : BaseComponent
{
    protected NewsFormComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    protected NewsFormComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }
}
