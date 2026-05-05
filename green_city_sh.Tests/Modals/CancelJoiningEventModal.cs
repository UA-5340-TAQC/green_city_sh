using Allure.Net.Commons.Attributes;
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

    [AllureStep("Click the 'Yes' button in the cancel joining event modal")]
    public void ClickYesButton() =>
        RootElement.FindElement(YesButton).Click();

    [AllureStep("Click the 'No' button in the cancel joining event modal")]
    public void ClickNoButton() =>
        RootElement.FindElement(NoButton).Click();

    [AllureStep("Click the 'Cancel' button in the cancel joining event modal")]
    public void ClickCancelButton() =>
        RootElement.FindElement(CancelButton).Click();
}