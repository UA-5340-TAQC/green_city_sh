using OpenQA.Selenium;
using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;
using OpenQA.Selenium.Support.UI;

namespace green_city_sh.Tests.Pages;

public abstract class BasePage : Base
{
    private HeaderComponent? header;
    public HeaderComponent Header => header ??= new HeaderComponent(driver, By.TagName("header"));
    
    protected BasePage(IWebDriver driver) : base(driver)
    {
    }

    public void Open(string url)
    {
        driver.Navigate().GoToUrl(url);
    }
    public string getTitle()
    {
        return driver.Title;
    }
    public string getUrl()
    {
        return driver.Url;
    }
    
    public void WaitUntilPageLoads() =>
        wait.Until(d =>
            ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState")?.Equals("complete") == true
            && d.FindElement(By.TagName("body")).Displayed);
}
