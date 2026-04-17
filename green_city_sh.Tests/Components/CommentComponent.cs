using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class CommentComponent : BaseComponent
{
    public CommentComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public CommentComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }
}