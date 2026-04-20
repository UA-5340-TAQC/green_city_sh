using OpenQA.Selenium;
using green_city_sh.Tests.Components;

namespace green_city_sh.Tests.Pages;

public class NewsPage : BasePage
{
    private By ItemsFoundText => By.CssSelector(".active-filter-container p");

    public NewsListComponent? NewsList { get; private set; }
    public NewsTopBarComponent? TopBar { get; private set; }
    public NewsFilterSelectionComponent? FilterSelection { get; private set; }

    public NewsPage(IWebDriver driver) : base(driver)
    {
    }

    public void SelectFilter(string filterName)
    {
        FilterSelection?.SelectFilter(filterName);
        Thread.Sleep(500); 
    }

    public void SelectFilters(params string[] filterNames)
    {
        foreach (var name in filterNames)
        {
            SelectFilter(name);
        }
    }

    public void SwitchToGridView()
    {
        NewsList?.SwitchToGridView();
    }

    public void SwitchToListView()
    {
        NewsList?.SwitchToListView();
    }

    public int GetNewsCardsCount()
    {
        return NewsList?.GetNewsCards() ?? 0;
    }

    public List<IWebElement> GetNewsCardElements()
    {
        return NewsList?.GetNewsCardElements() ?? new List<IWebElement>();
    }

    public void OpenNewsByIndex(int index)
    {
        NewsList?.OpenNewsByIndex(index);
    }
    
    public void ClickCreateNews()
    {
        TopBar?.ClickCreateNews();
    }
    
    public void OpenSearch()
    {
        TopBar?.OpenSearch();
    }
    
    public void SearchNews(string searchTerm)
    {
        TopBar?.SearchNews(searchTerm);
    }

    
    public void OpenSavedNews()
    {
        TopBar?.OpenSavedNews();
    }

    
    public void OpenMyNewsFilter()
    {
        TopBar?.OpenMyNews();
    }
    
    public bool IsFilterSelected(string filterName)
    {
        return FilterSelection?.IsFilterSelected(filterName) ?? false;
    }

    public void ClearAllFilters()
    {
        FilterSelection?.ClearAllFilters();
    }


    public void ScrollToLoadMore()
    {
        NewsList?.ScrolltoLoad();
    }
}

