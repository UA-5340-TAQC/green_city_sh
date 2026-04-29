using OpenQA.Selenium;
using green_city_sh.Tests.Infrastructure;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace green_city_sh.Tests.Components;

public abstract class BaseComponent : Base
{
    
    protected IWebDriver Driver;
    protected IWebElement RootElement;
    
    protected BaseComponent(IWebDriver driver, By rootLocator) : base(driver)
    {
        Driver = driver;
        RootElement = driver.FindElement(rootLocator);
    }
    protected BaseComponent(IWebDriver driver, IWebElement componentRoot) : base(driver)
    {
        Driver = driver;
        RootElement = componentRoot;
    }
    
    protected IWebElement FindElement(By locator) => RootElement.FindElement(locator);
    protected IList<IWebElement> FindElements(By locator) => RootElement.FindElements(locator);

    protected void WaitAndClick(By locator)
    {
        var element = wait.Until(d => RootElement.FindElement(locator));
        wait.Until(ExpectedConditions.ElementToBeClickable(element));
        element.Click();
    }

    protected void WaitAndTypeText(By locator, string text)
    {
        var element = wait.Until(d => RootElement.FindElement(locator));
        element.Clear();
        element.SendKeys(text);
    }

    protected void WaitUntilElementVisibleBy(By locator)
    {
        wait.Until(ExpectedConditions.ElementIsVisible(locator));
    }
    
    
}
