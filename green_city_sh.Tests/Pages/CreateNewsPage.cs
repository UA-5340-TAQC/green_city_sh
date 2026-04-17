using OpenQA.Selenium;
using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;

namespace green_city_sh.Tests.Pages;

public abstract class CreateNewsPage : BasePage
{
    protected CreateNewsPage(IWebDriver driver) : base(driver)
    {
    }
}
