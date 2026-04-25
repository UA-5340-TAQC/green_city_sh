using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using green_city_sh.Tests.Infrastructure;

namespace green_city_sh.Tests.Components;

public class MaterialCheckboxComponent : BaseComponent
{
    private readonly By _inputLocator = By.CssSelector("input[type='checkbox']");

    public MaterialCheckboxComponent(IWebDriver driver, By rootLocator) 
        : base(driver, rootLocator)
    {
    }

    public MaterialCheckboxComponent(IWebDriver driver, IWebElement componentRoot) 
        : base(driver, componentRoot)
    {
    }

    private IWebElement NativeInput =>
        RootElement.TagName.Equals("input", StringComparison.OrdinalIgnoreCase)
            ? RootElement
            : RootElement.FindElement(_inputLocator);

    public bool IsChecked()
    {
        return NativeInput.Selected;
    }

    public void Check()
    {
        if (!IsChecked())
        {
            Toggle();
        }
    }

    public void Uncheck()
    {
        if (IsChecked())
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        wait.Until(ExpectedConditions.ElementToBeClickable(RootElement));
        RootElement.Click();
    }
}