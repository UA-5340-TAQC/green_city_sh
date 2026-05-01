using System;
using System.Collections.Generic;
using System.Linq;
using green_city_sh.Tests.Infrastructure;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace green_city_sh.Tests.Components;

public class DropDownComponent : BaseComponent
{
    private const string OptionNameNotFound = "Dropdown option not found";
    
    private readonly By _searchLocator;
    // Updated to global scope (removed leading dot)
    private static readonly By DefaultOptions = By.XPath("//div[contains(@class,'cdk-overlay-pane')]//mat-option");
    private readonly By _selectedValueLocator = By.CssSelector(".mat-mdc-select-value-text");

    public DropDownComponent(IWebDriver driver, By rootLocator)
        : base(driver, rootLocator)
    {
        _searchLocator = DefaultOptions;
    }

    public DropDownComponent(IWebDriver driver, IWebElement componentRoot)
        : base(driver, componentRoot)
    {
        _searchLocator = DefaultOptions;
    }

    public string GetSelectedOptionText()
    {
        // Polling the element until Angular populates the text (prevents empty string assertions)
        var selectedValueElement = wait.Until(driver => 
        {
            try
            {
                var el = RootElement.FindElement(_selectedValueLocator);
                return !string.IsNullOrWhiteSpace(el.Text) ? el : null;
            }
            catch (StaleElementReferenceException)
            {
                return null; // Ignore stale elements and keep polling
            }
        });
        
        return selectedValueElement!.Text.Trim();
    }

    private IList<IWebElement> GetOptionList() =>
        wait.Until(drv =>
        {
            try
            {
                // Search globally and filter for interactable options
                var elements = drv.FindElements(_searchLocator)
                                  .Where(e => e.Displayed && e.Enabled)
                                  .ToList();
                return elements.Count > 0 ? elements : null;
            }
            catch (StaleElementReferenceException)
            {
                return null; // Implicit protection against DOM changes during filtering
            }
        })!;

    private IWebElement GetDropDownOptionByPartialName(string partialName)
    {
        if (string.IsNullOrWhiteSpace(partialName)) 
            throw new ArgumentException("Option cannot be empty", nameof(partialName));
        
        var options = GetOptionList();

        return options.FirstOrDefault(option => 
                   option.Text.Contains(partialName, StringComparison.OrdinalIgnoreCase))
               ?? throw new NoSuchElementException($"{OptionNameNotFound}: {partialName}");
    }

    public IList<string> GetListOptionsText() =>
        GetOptionList().Select(e => e.Text.Trim()).ToList();

    public void ClickDropDownOptionByPartialName(string partialName)
    {
        GetDropDownOptionByPartialName(partialName).Click();
    }

    public void Click()
    {
        wait.Until(ExpectedConditions.ElementToBeClickable(RootElement)).Click();
    }
}
