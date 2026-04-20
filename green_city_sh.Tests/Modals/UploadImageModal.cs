using green_city_sh.Tests.Components;
using OpenQA.Selenium;

namespace green_city_sh.Tests.Modals;

public class UploadImageModal : BaseModal
{
   
    private By CancelButton => By.CssSelector(".tertiary-global-button.m-btn"); 
    private By DeleteImgButton => By.CssSelector(".secondary-global-button.m-btn");
    private By UploadImgButton => By.CssSelector(".primary-global-button.m-btn");
    private By SaveImgButton => By.XPath(".//button[contains(text(), 'Зберегти фото') or contains(text(), 'Save photo')]");
    private By ChangeImgButton => By.XPath(".//button[contains(text(), 'Змінити фотографію') or contains(text(), 'Change photo')]");
    private By AvatarImg => By.XPath(".//img[contains(@class, 'ngx-ic-source-image')]");
    private By ModalTitle => By.XPath(".//p[@class= 'ng-star-inserted']");
    
   
    public UploadImageModal(IWebDriver driver, IWebElement rootElement) : base(driver, rootElement)
    {
    }

    public UploadImageModal(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }
    private ImageUploadComponent UploadComponent =>
        new ImageUploadComponent(driver, RootElement);

    public void ClickUploadButton() => 
        RootElement.FindElement(UploadImgButton).Click();   
    
    public void ClickDeleteButton() => 
        RootElement.FindElement(DeleteImgButton).Click();
    
    public void ClickCancelButton() => 
        RootElement.FindElement(CancelButton).Click();  
    
    public void ClickSaveImgButton() => 
        RootElement.FindElement(SaveImgButton).Click();
    
    public void ClickChangeImgButton() => 
        RootElement.FindElement(ChangeImgButton).Click();
    
    public bool IsImageDisplayed() => 
        RootElement.FindElement(AvatarImg).Displayed;
    
    public string GetModalTitleText => 
        RootElement.FindElement(ModalTitle).Text;
    
    public void UploadImage(string imagePath)
    {
        UploadComponent.Upload(imagePath);
    }
}