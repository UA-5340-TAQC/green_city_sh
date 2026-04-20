using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class NewsTopBarComponent : BaseComponent
{
    private By TopBarTitle => By.CssSelector(".main-header"); 
    private By SearchIcon => By.XPath("//span[@class='search-img']/.."); 
    private By SearchInputField => By.CssSelector(".place-input"); 
    private By BookmarkIcon => By.XPath("//span[@class='bookmark-img']/.."); 
    private By MyNewssIcon => By.XPath("//img[@class='my-events-img']/.."); 
    private By CreateNewButton => By.CssSelector(".create button"); 
    
    public NewsTopBarComponent (IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }
    public NewsTopBarComponent (IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }
}