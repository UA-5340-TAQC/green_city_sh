using green_city_sh.Tests.Infrastructure;
using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class DropDownComponent : BaseComponent
{
    private const string OptionNameNotFound = "Dropdown option not found";
    private readonly By _searchLocator;

    //Default Locator For Angular dropdown options
    private static readonly By DefaultOptions =
        By.XPath(".//div[contains(@class,'cdk-overlay-pane')]//mat-option");

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
    //Return list of dropdown options
    private IList<IWebElement> GetOptionList() =>
        wait.Until(_ =>
        {
            var elements = RootElement.FindElements(_searchLocator);
            return elements.Count > 0 ? elements : null;
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
}