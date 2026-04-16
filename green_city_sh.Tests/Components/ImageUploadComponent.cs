using OpenQA.Selenium;
using green_city_sh.Tests.Infrastructure;

namespace green_city_sh.Tests.Components;

public abstract class ImageUploadComponent : Base
{
    protected IWebElement RootElement;
    protected ImageUploadComponent(IWebDriver driver, By rootLocator) : base(driver)
    {
        RootElement = driver.FindElement(rootLocator);
    }
    protected ImageUploadComponent(IWebDriver driver, IWebElement componentRoot) : base(driver)
    {
        RootElement = componentRoot;
    }
}
