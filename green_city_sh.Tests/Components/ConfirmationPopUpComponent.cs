using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class ConfirmationPopUpComponent : BaseComponent
{
    public ConfirmationPopUpComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public ConfirmationPopUpComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }
}