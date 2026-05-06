using Allure.Net.Commons.Attributes;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace green_city_sh.Tests.Modals;

public class DeleteCommentModal : BaseModal
{
    private By WarningText => By.CssSelector(".warning-title");
    private By CancelBtn => By.CssSelector(".m-btn.secondary-global-button");
    private By YesBtn => By.XPath(".//button[contains(@class,'m-btn primary-global-button')]");

    public DeleteCommentModal(IWebDriver driver, IWebElement rootElement) : base(driver, rootElement)
    {
    }

    public DeleteCommentModal(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    [AllureStep("Click 'yes delete' button")]
    public void ClickYesDeleteBtn()
    {
        WaitAndClick(YesBtn);
        wait.Until(ExpectedConditions.StalenessOf(driver.FindElement(YesBtn)));
    }

    [AllureStep("Click 'cancel' button")]
    public void ClickCancelDeleteBtn() =>
        WaitAndClick(CancelBtn);
    public string GetWarningText() =>
        RootElement.FindElement(WarningText).Text;

}