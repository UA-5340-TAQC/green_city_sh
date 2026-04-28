using OpenQA.Selenium;
using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;

namespace green_city_sh.Tests.Pages;

public class HomePage : BasePage
{
    private HeaderComponent? header;

    public HomePage(IWebDriver driver) : base(driver)
    {
    }
}
