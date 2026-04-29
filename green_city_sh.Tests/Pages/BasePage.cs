using OpenQA.Selenium;
using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;
using OpenQA.Selenium.Support.UI;

namespace green_city_sh.Tests.Pages;

public abstract class BasePage : Base
{
    protected BasePage(IWebDriver driver) : base(driver)
    {
    }

    public void Open(string url)
    {
        driver.Navigate().GoToUrl(url);
    }
    public string GetTitle()
    {
        return driver.Title;
    }
    public string GetUrl()
    {
        return driver.Url;
    }

    public void Refresh() =>
        driver.Navigate().Refresh();

    public void WaitUntilPageLoads() =>
        wait.Until(d =>
            ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState")?.Equals("complete") == true
            && d.FindElement(By.TagName("body")).Displayed);
}
