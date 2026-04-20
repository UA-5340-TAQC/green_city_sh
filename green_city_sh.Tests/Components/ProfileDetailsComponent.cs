using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class ProfileDetailsComponent : BaseComponent
{ 
        private By NameField => By.Id("name");
        private By CityNameField => By.CssSelector("input[role='combobox']"); 
        private By CredoField => By.Id("credo");
        private By CityDropDownOptions => By.XPath(".//div[contains(@class, 'cdk-overlay-pane')]//mat-option");
        
        public ProfileDetailsComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
        {
        }

        public ProfileDetailsComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
        {
        }

        public void EnterName(string name)
        {
                WaitAndTypeText(NameField, name);
                //Enter a valid name
        }

        public void EnterCityName(string name)
        {
                WaitAndTypeText(CityNameField, name);
                Wait.Until(d => d.FindElements(CityDropDownOptions).Count > 0);
                var dropdown = new DropDownComponent(driver, By.TagName("body"));
                dropdown.ClickDropDownOptionByPartialName(name);
        }

        public void EnterCredo(string credo)
        {
                WaitAndTypeText(CredoField, credo);
                //Enter a valid credo text 
        }
}
