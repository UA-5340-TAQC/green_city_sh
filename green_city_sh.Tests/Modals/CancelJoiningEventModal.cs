using green_city_sh.Tests.Components;
using OpenQA.Selenium;

namespace green_city_sh.Tests.Modals;

public class CancelJoiningEventModal : BaseModal
{
    private By CancelButton => By.XPath(".//button[@class='close ng-star-inserted']"); 
    private By YesButton => By.XPath(".//button[@class='m-btn primary-global-button']");
    private By NoButton => By.XPath(".//button[@class='m-btn secondary-global-button']");

    public CancelJoiningEventModal(IWebDriver driver, IWebElement rootElement) : base(driver, rootElement)
    {
    }

    public CancelJoiningEventModal(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public void ClickYesButton() => 
        RootElement.FindElement(YesButton).Click();

    public void ClickNoButton() => 
        RootElement.FindElement(NoButton).Click();

    public void ClickCancelButton() => 
        RootElement.FindElement(CancelButton).Click();
}