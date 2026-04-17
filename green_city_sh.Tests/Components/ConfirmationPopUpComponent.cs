using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class ConfirmationPop_UpComponent : BaseComponent
{
    public ConfirmationPop_UpComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public ConfirmationPop_UpComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }
}