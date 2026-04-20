using OpenQA.Selenium;
using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;
using OpenQA.Selenium.Support.UI;

namespace green_city_sh.Tests.Pages;

public abstract class BasePage : Base
{
    protected WebDriverWait Wait; 
    protected BasePage(IWebDriver driver) : base(driver)
    {
        Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
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
        Wait.Until(d => d.FindElement(By.TagName("body")).Displayed);
}
