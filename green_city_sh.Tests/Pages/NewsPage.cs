using OpenQA.Selenium;
using green_city_sh.Tests.Components;

namespace green_city_sh.Tests.Pages;

public class NewsPage : BasePage
{
    public NewsTopBarComponent TopBar { get; }
    public NewsFilterSelectionComponent Filters { get; }
    public NewsTagsComponent TagsFilter { get; }
    public NewsListComponent List { get; }

    private By TopBarRoot => By.CssSelector(".main-header");
    private By FilterRoot => By.CssSelector(".ul-eco-buttons");
    private By ListRoot => By.CssSelector(".list-wrapper");
    private By ItemsFoundCounter => By.CssSelector("app-remaining-count h2");

    public NewsPage(IWebDriver driver) : base(driver)
    {
        TopBar = new NewsTopBarComponent(driver, driver.FindElement(TopBarRoot));
        Filters = new NewsFilterSelectionComponent(driver, driver.FindElement(FilterRoot));
        TagsFilter = new NewsTagsComponent(driver, driver.FindElement(FilterRoot));
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

    /// <summary>
    /// Returns a text for the items found counter.
    /// </summary>
    public string GetItemsFoundText()
    {
        wait.Until(driver => driver.FindElement(ItemsFoundCounter).Text.Length > 0);
        return driver.FindElement(ItemsFoundCounter).Text.Trim();
    }

    /// <summary>
    /// Returns a number for the items found counter.
    /// </summary>
    public int GetItemsFoundCount()
    {
        var text = GetItemsFoundText();
        var parts = text.Split(' ');
        if (int.TryParse(parts[0], out int count))
        {
            return count;
        }
        return 0;
    }

    /// <summary>
    /// Waits until the items found counter updates to a different number than the provided previous count.
    /// </summary>
    public void WaitForSearchResultsToUpdate(int previousCount)
    {
        wait.Until(driver => GetItemsFoundCount() != previousCount);
    }
}