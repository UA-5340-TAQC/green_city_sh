using OpenQA.Selenium;
using Allure.NUnit.Attributes;

namespace green_city_sh.Tests.Components;

public class EditProfileButtonComponent : BaseComponent
{
    private By SaveBtn => By.XPath(".//button[contains(@class, 'primary-global-button')]");
    private By CancelBtn => By.XPath(".//button[contains(@class, 'secondary-global-button')]");

    public EditProfileButtonComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public EditProfileButtonComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }
    [AllureStep("Click Save button in Edit Profile component")]
    public void ClickSaveBtn() =>
        WaitAndClick(SaveBtn);

    [AllureStep("Click Cancel button in Edit Profile component")]
    public void ClickCancelBtn() =>
        WaitAndClick(CancelBtn);

    [AllureStep("Check if Save button is enabled in Edit Profile component")]
    public bool IsSaveBtnEnabled() =>
        FindElement(SaveBtn).Enabled;

    [AllureStep("Check if Cancel button is enabled in Edit Profile component")]
    public bool IsCancelBtnEnabled() =>
        FindElement(CancelBtn).Enabled;
    [AllureStep("Check if Cancel button is displayed in Edit Profile component")]
    public bool IsCancelBtnDisplayed() =>
        FindElement(CancelBtn).Displayed;

    [AllureStep("Check if Save button is displayed in Edit Profile component")]
    public bool IsSaveBtnDisplayed() =>
        FindElement(SaveBtn).Displayed;
}
