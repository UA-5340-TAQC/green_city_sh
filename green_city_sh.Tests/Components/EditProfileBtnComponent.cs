using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class EditProfileButtonComponent: BaseComponent
{
    public EditProfileButtonComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public EditProfileButtonComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }
}
