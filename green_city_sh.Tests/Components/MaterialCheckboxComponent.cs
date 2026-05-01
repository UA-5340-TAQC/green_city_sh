using System;
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
        string classAttribute = RootElement.GetAttribute("class") ?? string.Empty;
        
        // Modern MDC-based Angular Material applies "mat-mdc-checkbox-checked", 
        // while older versions apply "mat-checkbox-checked" directly to the host element.
        return classAttribute.Contains("mat-mdc-checkbox-checked") || 
               classAttribute.Contains("mat-checkbox-checked");
    }

    public void Check()
    {
        if (!IsChecked())
        {
            Toggle();
            // Explicitly wait for Angular's state update and animation to complete
            wait.Until(_ => IsChecked());
        }
    }

    public void Uncheck()
    {
        if (IsChecked())
        {
            Toggle();
            // Explicitly wait for Angular's state update and animation to complete
            wait.Until(_ => !IsChecked());
        }
    }

    public void Toggle()
    {
        wait.Until(ExpectedConditions.ElementToBeClickable(RootElement));

        // Use JS-click on the hidden native input to trigger Angular's internal state correctly
        var js = (IJavaScriptExecutor)driver;
        js.ExecuteScript("arguments[0].click();", NativeInput);
    }
}