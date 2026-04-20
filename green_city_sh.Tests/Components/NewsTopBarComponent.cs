using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class NewsTopBarComponent : BaseComponent
{
    private By TopBarTitle => By.CssSelector(".main-header");
    private By SearchIcon => By.XPath("//span[@class='search-img']/..");
    private By SearchInputField => By.CssSelector(".place-input");
    private By BookmarkIcon => By.XPath("//span[@class='bookmark-img']/..");
    private By MyNewsIcon => By.XPath("//img[@class='my-events-img']/..");
    private By CreateNewButton => By.CssSelector(".create button");

    public NewsTopBarComponent (IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public NewsTopBarComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }

    public void OpenSearch()
    {
        try
        {
            var searchBtn = RootElement.FindElement(SearchIcon);
            searchBtn.Click();
        }
        catch (NoSuchElementException)
        {
            // Search button not found
        }
    }
    
    public void SearchNews(string searchTerm)
    {
        OpenSearch();
        try
        {
            var searchInput = RootElement.FindElement(SearchInputField);
            searchInput.SendKeys(searchTerm);
            searchInput.SendKeys(Keys.Enter);
        }
        catch (NoSuchElementException)
        {
            // Search input not found
        }
    }
    
    public void OpenSavedNews()
    {
        try
        {
            var bookmarkBtn = RootElement.FindElement(BookmarkIcon);
            bookmarkBtn.Click();
        }
        catch (NoSuchElementException)
        {
            // Bookmark button not found
        }
    }
    
    public void OpenMyNews()
    {
        try
        {
            var calendarBtn = RootElement.FindElement(MyNewsIcon);
            calendarBtn.Click();
        }
        catch (NoSuchElementException)
        {
            // Calendar button not found
        }
    }
    
    
    public void ClickCreateNews()
    {
        try
        {
            var createBtn = RootElement.FindElement(CreateNewButton);
            createBtn.Click();
        }
        catch (NoSuchElementException)
        {
            // Create button not found
        }
    }
}