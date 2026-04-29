using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class PrivacySettingsComponent : BaseComponent
{
    private By PrivacyDropdown(string type) => By.XPath($".//li[.//div[contains(., '{type}')]]//mat-select");

    public PrivacySettingsComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public PrivacySettingsComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }

    public void SelectPrivacy(string category, string value)
    {
        WaitAndClick(PrivacyDropdown(category));
        var dropdown = new DropDownComponent(driver, By.TagName("body"));
        dropdown.ClickDropDownOptionByPartialName(value);
    }
}
