using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class SignInModalComponent : BaseComponent
{
    public SignInModalComponent(IWebDriver driver, By rootLocator)
        : base(driver, rootLocator)
    {
    }

    public SignInModalComponent(IWebDriver driver, IWebElement rootElement)
        : base(driver, rootElement)
    {
    }
}