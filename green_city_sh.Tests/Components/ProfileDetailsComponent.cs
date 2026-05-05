using OpenQA.Selenium;
using Allure.NUnit.Attributes;

namespace green_city_sh.Tests.Components;

public class ProfileDetailsComponent : BaseComponent
{
    private By NameField => By.Id("name");
    private By CityNameField => By.CssSelector("input[role='combobox']");
    private By CredoField => By.Id("credo");

    public ProfileDetailsComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public ProfileDetailsComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }

    [AllureStep("Enter name in profile details component")]
    public void EnterName(string name)
    {
        WaitAndTypeText(NameField, name);
    }

    [AllureStep("Enter city name in profile details component")]
    public void EnterCityName(string name)
    {
        WaitAndTypeText(CityNameField, name);
        var dropdown = new DropDownComponent(driver, By.TagName("body"));
        //dropdown.WaitUntilVisible();
        dropdown.ClickDropDownOptionByPartialName(name);
        wait.Until(d =>
        {
            return dropdown.GetOptionList().Count > 0;
              //.Any(o => o.Displayed && o.Text.Contains(name)));
        });
        dropdown.ClickDropDownOptionByPartialName(name);
    }

    [AllureStep("Enter credo in profile details component")]
    public void EnterCredo(string credo)
    {
        WaitAndTypeText(CredoField, credo);
    }
}
