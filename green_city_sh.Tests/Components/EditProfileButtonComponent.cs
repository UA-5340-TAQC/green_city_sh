using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class EditProfileButtonComponent: BaseComponent
{
    private By SaveBtn => By.XPath(".//button[contains(@class, 'primary-global-button')]");
    private By CancelBtn => By.XPath(".//button[contains(@class, 'secondary-global-button')]");
    public EditProfileButtonComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public EditProfileButtonComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }
    
    public void ClickSaveBtn() =>
        WaitAndClick(SaveBtn);

    public void ClickCancelBtn() =>
        WaitAndClick(CancelBtn);
    
    public bool IsSaveBtnEnabled() => 
        FindElement(SaveBtn).Enabled;
    
    public bool IsCancelBtnEnabled() => 
        FindElement(CancelBtn).Enabled;
    
    public bool IsCancelBtnDisplayed() => 
        FindElement(CancelBtn).Displayed;
    
    public bool IsSaveBtnDisplayed() => 
        FindElement(SaveBtn).Displayed;
}
