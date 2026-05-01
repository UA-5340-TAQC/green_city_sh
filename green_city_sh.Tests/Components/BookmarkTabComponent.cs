using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class BookmarkTabComponent : BaseComponent
{
    public BookmarkTabComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public BookmarkTabComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }

    private By TabButtons => By.CssSelector(".tabs button");

    public void SwitchToTab(string tabName)
    {
        var allTabs = RootElement.FindElements(TabButtons);

        var targetTab = allTabs.FirstOrDefault(tab => 
            tab.Text.Contains(tabName, StringComparison.OrdinalIgnoreCase));

        if (targetTab != null)
        {
            targetTab.Click();
        }
    }
}