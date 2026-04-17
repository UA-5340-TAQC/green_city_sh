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
}