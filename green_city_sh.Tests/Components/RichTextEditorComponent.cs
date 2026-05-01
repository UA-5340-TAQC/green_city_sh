using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class RichTextEditorComponent : BaseComponent
{
    public RichTextEditorComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public RichTextEditorComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }
}