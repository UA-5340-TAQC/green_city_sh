namespace green_city_sh.Tests.Components;

public class SignUpModalComponent : BaseComponent
{
    public SignUpModalComponent(OpenQA.Selenium.IWebDriver driver, OpenQA.Selenium.By rootLocator)
        : base(driver, rootLocator)
    {
    }

    public SignUpModalComponent(OpenQA.Selenium.IWebDriver driver, OpenQA.Selenium.IWebElement rootElement)
        : base(driver, rootElement)
    {
    }
}