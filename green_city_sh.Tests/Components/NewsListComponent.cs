using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class NewsListComponent : BaseComponent
{
    private By AllNewsCars => By.CssSelector(".list-wrapper");
    
    public NewsListComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }
    public NewsListComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }
    
    public int GetNewsCardCount()
    {
        return 1;
    }

    public void SwitchToGridView()
    {
        
    }
    public void SwitchToListView()
    {
    }
}