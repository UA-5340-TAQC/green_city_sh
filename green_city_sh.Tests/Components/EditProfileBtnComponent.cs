using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class EditProfileBtnComponent: BaseComponent
{
    public EditProfileBtnComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public EditProfileBtnComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }
}
