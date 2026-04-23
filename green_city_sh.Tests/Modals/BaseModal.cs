using green_city_sh.Tests.Infrastructure;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace green_city_sh.Tests.Modals;

public class BaseModal : Base
{
    protected IWebElement RootElement;
    
    protected BaseModal(IWebDriver driver, IWebElement rootElement) : base(driver)
    { 
        RootElement = rootElement;
    }
    protected BaseModal(IWebDriver driver, By rootLocator) : base(driver)
    {
        RootElement = wait.Until(d =>
        {
            try
            {
                var element = d.FindElement(rootLocator);
                return element.Displayed ? element : null;
            }
            catch (NoSuchElementException)
            {
                return null;
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
        }) ?? throw new InvalidOperationException();
    }
    
    protected void WaitAndClick(By locator)
    {
        var element = wait.Until(ExpectedConditions.ElementToBeClickable(locator));
        element.Click();
    }
}
