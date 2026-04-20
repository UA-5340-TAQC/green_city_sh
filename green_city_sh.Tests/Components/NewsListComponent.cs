using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class NewsListComponent : BaseComponent
{
    private By AllNewsCars => By.CssSelector(".list-wrapper");
    private By GridViewButton => By.CssSelector(".btn-tiles");
    private By ListViewButton => By.CssSelector(".btn-bars");

    
    public NewsListComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }
    public NewsListComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }
    
    public int GetNewsCards()
    {
        return 1;
    }

    public void SwitchToGridView()
    {

    }
    public void SwitchToListView()
    {
    }
    public void OpenNewsByIndex(int index)
    {
    }
    

}