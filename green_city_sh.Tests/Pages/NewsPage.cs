using OpenQA.Selenium;
using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;

namespace green_city_sh.Tests.Pages;

public class NewsPage: BasePage
{
    private By ItemsFoundText => By.CssSelector(".active-filter-container p");
    private By FilterButtons => By.CssSelector(".ul-eco-buttons .tag-button");

    
    public NewsListComponent NewsList { get; private set;}
    public NewsTopBarComponent TopBar { get; private set;}
    
    public NewsPage(IWebDriver driver) : base(driver)
    {
    }

    public void SelectFilter(string filterName)
    {

    }
}

