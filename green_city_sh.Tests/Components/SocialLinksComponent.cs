using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class SocialLinksComponent : BaseComponent
{
    private By AddSocialLinkButton => By.XPath(".//*[contains(@class, 'add-button')]"); 
    private By AddButton => By.XPath(".//button[@class='primary-global-button']");
    private By CancelButton => By.XPath(".//button[@class='secondary-global-button']");
    private By LinkField => By.Id("socialLink");

    public SocialLinksComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public SocialLinksComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }

    public void ClickAddSocialLinkButton() => 
        WaitAndClick(AddSocialLinkButton);
    
    public void ClickAddLinkButton() =>  
        WaitAndClick(AddButton);

    public void ClickCancelButton() =>  
        WaitAndClick(CancelButton);  
    
    public bool IsSocialLinkBtnEnabled => 
        FindElement(AddSocialLinkButton).Enabled;

    public void EnterLink(string link) => 
        WaitAndTypeText(LinkField, link);
}
