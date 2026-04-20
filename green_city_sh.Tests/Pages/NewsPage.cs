using OpenQA.Selenium;
using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;

namespace green_city_sh.Tests.Pages;

public class NewsPage: BasePage
{
    private By ItemsFoundText => By.CssSelector(".active-filter-container p"); 
    private By GridViewButton => By.CssSelector(".btn-tiles");
    private By ListViewButton => By.CssSelector(".btn-bars"); 
    
    public NewsListComponent NewsList { get; private set;}
    public NewsTopBarComponent TopBar { get; private set;}
    public NewsFilterSelectionComponent FilterSelection { get; private set;}
    
    public NewsPage(IWebDriver driver) : base(driver)
    {
    }
}