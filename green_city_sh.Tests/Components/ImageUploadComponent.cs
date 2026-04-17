using OpenQA.Selenium;
using green_city_sh.Tests.Infrastructure;

namespace green_city_sh.Tests.Components;

public abstract class ImageUploadComponent : BaseComponent
{
    protected ImageUploadComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    protected ImageUploadComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }
}
