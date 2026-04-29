using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class EmailNotificationsComponent : BaseComponent
{
    // Locator for checkbox by notification type
    private By CheckboxBtn(string type) =>
        By.XPath($".//label[contains(normalize-space(.), '{type}')]/input[@type='checkbox']");
    //Locator for dropdown by notification type
    private By NotificationDropdown(string type) =>
            By.XPath($".//div[contains(@class,'email-preference-item')][.//label[contains(normalize-space(.), '{type}')]]//mat-select");

    public EmailNotificationsComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public EmailNotificationsComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }

    public void SetNotificationFrequency(string type, string frequency)
    {
        FindElement(NotificationDropdown(type)).Click();
        var dropdown = new DropDownComponent(driver, By.TagName("body"));
        dropdown.ClickDropDownOptionByPartialName(frequency);
    }

    public void ToggleNotificationFrequency(string type, bool enabled)
    {
        var checkbox = FindElement(CheckboxBtn(type));
        var isChecked = checkbox.Selected;
        if (isChecked != enabled)
        {
            checkbox.Click();
        }
    }
}