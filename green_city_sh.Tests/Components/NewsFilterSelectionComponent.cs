using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class NewsFilterSelectionComponent : BaseComponent
{
    private By FilterButtons => By.CssSelector(".ul-eco-buttons .tag-button");

    public NewsFilterSelectionComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public NewsFilterSelectionComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }

    /// <summary>
    /// Selects a filter button by its text name
    /// </summary>
    /// <param name="filterName">Filter name to select (e.g., "News", "Events", "Education", "Initiatives", "Ads")</param>
    public void SelectFilter(string filterName)
    {
        var filters = RootElement.FindElements(FilterButtons);
        var filterButton = filters.FirstOrDefault(f => f.Text.Trim().Equals(filterName, StringComparison.OrdinalIgnoreCase));
        
        if (filterButton != null)
        {
            filterButton.Click();
        }
        else
        {
            throw new NoSuchElementException($"Filter '{filterName}' not found. Available filters: {string.Join(", ", filters.Select(f => f.Text))}");
        }
    }

    /// <summary>
    /// Selects multiple filters
    /// </summary>
    public void SelectFilters(params string[] filterNames)
    {
        foreach (var filterName in filterNames)
        {
            SelectFilter(filterName);
        }
    }

    /// <summary>
    /// Gets all available filter names
    /// </summary>
    public List<string> GetAvailableFilters()
    {
        return RootElement.FindElements(FilterButtons)
            .Select(f => f.Text.Trim())
            .ToList();
    }

    /// <summary>
    /// Checks if a specific filter is selected
    /// </summary>
    public bool IsFilterSelected(string filterName)
    {
        var filters = RootElement.FindElements(FilterButtons);
        var filterButton = filters.FirstOrDefault(f => f.Text.Trim().Equals(filterName, StringComparison.OrdinalIgnoreCase));
        
        if (filterButton != null)
        {
            return filterButton.GetAttribute("class").Contains("active") || filterButton.GetAttribute("aria-pressed") == "true";
        }

        return false;
    }

    /// <summary>
    /// Clears all selected filters
    /// </summary>
    public void ClearAllFilters()
    {
        var filters = RootElement.FindElements(FilterButtons);
        foreach (var filter in filters)
        {
            if (filter.GetAttribute("class").Contains("active"))
            {
                filter.Click();
                Thread.Sleep(300); // Wait for filter to update
            }
        }
    }
}

