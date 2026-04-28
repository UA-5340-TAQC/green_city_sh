using OpenQA.Selenium;
using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;

namespace green_city_sh.Tests.Pages;

public class NewsPage : BasePage
{
    public NewsTopBarComponent TopBar { get;}
    public NewsTagsComponent TagsFilter { get;}
    public NewsFilterSelectionComponent Filters { get;  }
    public NewsListComponent List { get;}

    private By TopBarRoot => By.CssSelector(".main-header");
    private By FilterRoot => By.CssSelector("app-tag-filter .ul-eco-buttons");
    private By ListRoot => By.CssSelector(".list-wrapper");

    private By ItemsFoundCounter => By.CssSelector("app-remaining-count h2");

    public NewsPage(IWebDriver driver) : base(driver)
    {
        TopBar = new NewsTopBarComponent(driver, driver.FindElement(TopBarRoot));
        TagsFilter = new NewsTagsComponent(driver, By.CssSelector(".wrapper"));
        Filters = new NewsFilterSelectionComponent(driver, driver.FindElement(FilterRoot));
        List = new NewsListComponent(driver, driver.FindElement(ListRoot));
    }


    public void FilterBy(string name)
    {
        Filters.SelectFilter(name);
    }

    public void FilterBy(params string[] names)
    {
        foreach (var n in names)
            Filters.SelectFilter(n);
    }

    public void SwitchToGrid() => List.SwitchToGridView();
    public void SwitchToList() => List.SwitchToListView();

    public void ScrollForMore() => List.ScrollToLoad();

    public bool IsFilterActive(string name)
        => Filters.IsFilterSelected(name);

    public void ClearFilters()
        => Filters.ClearAllFilters();

    public void Search(string text)
        => TopBar.SearchNews(text);

    public void OpenSaved()
        => TopBar.OpenSavedNews();
}
