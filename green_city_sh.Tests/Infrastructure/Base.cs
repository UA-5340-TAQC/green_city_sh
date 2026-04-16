using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace green_city_sh.Tests.Infrastructure;

public abstract class Base
{
    protected IWebDriver driver;
    protected WebDriverWait wait;

    protected Base(IWebDriver driver)
    {
        this.driver = driver;
        this.wait = new WebDriverWait(driver,
            TimeSpan.FromSeconds(Configuration.DefaultTimeout));
    }
}
