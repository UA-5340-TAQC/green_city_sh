using OpenQA.Selenium;

namespace green_city_sh.Tests.Modals;

public class DeleteCommentModal : BaseModal
{
    private By WarningText => By.CssSelector(".warning-title");
    private By CancelBtn => By.CssSelector(".m-btn.secondary-global-button");
    private By YesBtn => By.CssSelector(".m-btn.primary-global-button");
    
    public DeleteCommentModal(IWebDriver driver, IWebElement rootElement) : base(driver, rootElement)
    {
    }

    public DeleteCommentModal(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public void ClickYesDeleteBtn()
    {
        WaitAndClick(YesBtn);
    }

    public void ClickCancelDeleteBtn() =>
        WaitAndClick(CancelBtn);
    public string GetWarningText() => 
        RootElement.FindElement(WarningText).Text;
    
}