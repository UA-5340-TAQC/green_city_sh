using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class NewsTopBarComponent : BaseComponent
{
    private By TopBarTitle => By.CssSelector(".main-header");
    private By SearchIcon => By.XPath(".//div[contains(@class, 'container-img') and .//span[@class='search-img']]");
    private By SearchInputField => By.CssSelector(".place-input");
    private By BookmarkIcon => By.XPath(".//span[@class='bookmark-img']/..");
    private By MyNewsIcon => By.XPath(".//img[@class='my-events-img']/..");
    private By CreateNewButton => By.CssSelector(".create button");

    public NewsTopBarComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public NewsTopBarComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }

    public void OpenSearch()
    {
        wait.Until(d => d.FindElement(SearchIcon)).Click();
    }

    public void SearchNews(string searchTerm)
    {
        OpenSearch();
        var searchInput = wait.Until(d => d.FindElement(SearchInputField));
        searchInput.Clear();
        foreach (char c in searchTerm)
        {
            searchInput.SendKeys(c.ToString());
            Thread.Sleep(100);
        }
    }

    public void OpenSavedNews()
    {
        var bookmarkBtn = RootElement.FindElement(BookmarkIcon);
        bookmarkBtn.Click();
    }

    public void OpenMyNews()
    {
        var calendarBtn = RootElement.FindElement(MyNewsIcon);
        calendarBtn.Click();
    }


    public void ClickCreateNews()
    {
        var createBtn = RootElement.FindElement(CreateNewButton);
        createBtn.Click();
    }
}
