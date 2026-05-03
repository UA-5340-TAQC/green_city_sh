using OpenQA.Selenium;
using System.Text.RegularExpressions;
using OpenQA.Selenium.Support.UI;
using System;
using Allure.NUnit.Attributes;

namespace green_city_sh.Tests.Components;

public class NewsTopBarComponent : BaseComponent
{
    private By TopBarTitle => By.CssSelector(".main-header");
    private By SearchIcon => By.XPath(".//span[@class='search-img']/..");
    private By SearchInputField => By.CssSelector(".place-input");
    private By BookmarkIcon => By.XPath("//div[contains(@class, 'container-img') and .//span[contains(@class, 'bookmark-img')]]");
    private By MyNewsIcon => By.XPath(".//img[@class='my-events-img']/..");
    private By CreateNewButton => By.CssSelector(".create button");

    public NewsTopBarComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public NewsTopBarComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }

    [AllureStep("Get news page title")]
    public void OpenSearch()
    {
        var searchBtn = RootElement.FindElement(SearchIcon);
        searchBtn.Click();
    }

    [AllureStep("Search for news")]
    public void SearchNews(string searchTerm)
    {
        OpenSearch();
        var searchInput = RootElement.FindElement(SearchInputField);
        searchInput.SendKeys(searchTerm);
        searchInput.SendKeys(Keys.Enter);
    }

    [AllureStep("Open saved news")]
    public void OpenSavedNews()
    {
        var bookmarkBtn = wait.Until(d => d.FindElement(BookmarkIcon));

        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
        js.ExecuteScript("arguments[0].click();", bookmarkBtn);
    }

    [AllureStep("Open my news")]
    public void OpenMyNews()
    {
        var calendarBtn = RootElement.FindElement(MyNewsIcon);
        calendarBtn.Click();
    }

    [AllureStep("Click create news button")]
    public void ClickCreateNews()
    {
        var createBtn = RootElement.FindElement(CreateNewButton);
        createBtn.Click();
    }

    [AllureStep("Get count of items found in search results")]
    public int GetItemsFoundCount()
    {
        var countElements = driver.FindElements(By.XPath("//h2[contains(text(), 'item')]"));

        if (countElements.Count == 0) return -1;

        string text = countElements[0].Text.Trim();
        string numberOnly = text.Split(' ')[0];

        if (int.TryParse(numberOnly, out int result))
        {
            return result;
        }

        return 0;
    }

    [AllureStep("Wait for items found counter to appear")]
    public void WaitForCounterToAppear()
    {
        wait.Until(d => GetItemsFoundCount() >= 0);
    }
}