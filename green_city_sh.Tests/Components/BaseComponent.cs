using OpenQA.Selenium;
using green_city_sh.Tests.Infrastructure;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace green_city_sh.Tests.Components;

public abstract class BaseComponent : Base
{
    protected IWebElement RootElement;
    protected WebDriverWait Wait;
    protected BaseComponent(IWebDriver driver, By rootLocator) : base(driver)
    {
        RootElement = driver.FindElement(rootLocator);
        Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
    }
    protected BaseComponent(IWebDriver driver, IWebElement componentRoot) : base(driver)
    {
        RootElement = componentRoot;
        Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
    }
    
    protected IWebElement FindElement(By locator) => RootElement.FindElement(locator);
    protected IList<IWebElement> FindElements(By locator) => RootElement.FindElements(locator);

    protected void WaitAndClick(By locator)
    {
        var element = Wait.Until(d => RootElement.FindElement(locator));
        Wait.Until(ExpectedConditions.ElementToBeClickable(element));
        element.Click();
    }
        

    protected void WaitAndTypeText(By locator, string text)
    {
        var element = Wait.Until(d => RootElement.FindElement(locator));
        Wait.Until(ExpectedConditions.ElementToBeClickable(element));
        element.Clear();
        element.SendKeys(text);
    }

    public void WaitUntilElementVisibleBy(By locator)
    {
        Wait.Until(d => RootElement.FindElement(locator).Displayed);
    }
    
    
}
