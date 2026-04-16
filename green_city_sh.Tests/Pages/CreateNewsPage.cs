using OpenQA.Selenium;
using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;

namespace green_city_sh.Tests.Pages;

public abstract class CreateNewsPage : Base
{
    protected CreateNewsPage(IWebDriver driver) : base(driver)
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
}
