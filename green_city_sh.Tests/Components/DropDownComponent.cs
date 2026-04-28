using green_city_sh.Tests.Infrastructure;
using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class DropDownComponent : BaseComponent
{
    private const string OptionNameNotFound = "Dropdown option not found";
    private readonly By _searchLocator;


    private static readonly By DefaultOptions =
    By.XPath(".//div[contains(@class,'cdk-overlay-pane')]//mat-option | .//div[contains(@class,'pac-container')]//div[contains(@class,'pac-item')]");


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


    private IList<IWebElement> GetOptionList()
    {
        return wait.Until(_ =>
        {
            var elements = RootElement
                .FindElements(_searchLocator)
                .Where(option =>
                {
                    try
                    {
                        return option.Displayed && !string.IsNullOrWhiteSpace(option.Text);
                    }
                    catch (StaleElementReferenceException)
                    {
                        return false;
                    }
                })
                .ToList();

            return elements.Count > 0 ? elements : null;
        })!;
    }

    private IWebElement GetDropDownOptionByPartialName(string partialName)
    {
        if (string.IsNullOrWhiteSpace(partialName))
            throw new ArgumentException("Option cannot be empty", nameof(partialName));

        var options = GetOptionList();

        var optionTexts = string.Join(", ", options.Select(o => o.Text.Trim()));
        TestContext.WriteLine($"Dropdown options: {optionTexts}");

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