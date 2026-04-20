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

    public void SelectFilter(string filterName)
    {
        //select buttons
    }
}
    
    
 