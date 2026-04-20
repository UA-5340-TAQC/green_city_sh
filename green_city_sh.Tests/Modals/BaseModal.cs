using green_city_sh.Tests.Infrastructure;
using OpenQA.Selenium;

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
        RootElement = driver.FindElement(rootLocator);
    }
}