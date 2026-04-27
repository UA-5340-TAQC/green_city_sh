using OpenQA.Selenium;

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

        public void EnterName(string name)
        {
                WaitAndTypeText(NameField, name);
        }

        public void EnterCityName(string name)
        {
        var input = FindElement(CityNameField);

        input.Click();
        input.SendKeys(Keys.Control + "a");
        input.SendKeys(Keys.Backspace);
        input.SendKeys(name);

        var dropdown = new DropDownComponent(driver, By.TagName("body"));
        dropdown.ClickDropDownOptionByPartialName(name);
        }


    public void EnterCredo(string credo)
        {
            WaitAndTypeText(CredoField, credo);
        }

        /// <summary>
        /// Retrieves the current value of the Name field.
        /// </summary>
        /// <returns>
        /// The value of the Name input field, or an empty string if the value is null.
        /// </returns>
        public string GetName()
        {
            return FindElement(NameField).GetAttribute("value") ?? "";
        }

        /// <summary>
        /// Retrieves the current value of the City field.
        /// </summary>
        /// <returns>
        /// The value of the City input field, or an empty string if the value is null.
        /// </returns>
        public string GetCityName()
        {
            return FindElement(CityNameField).GetAttribute("value") ?? "";
        }

        /// <summary>
        /// Retrieves the current value of the Credo field.
        /// </summary>
        /// <returns>
        /// The value of the Credo input field, or an empty string if the value is null.
        /// </returns>
        public string GetCredo()
        {
            return FindElement(CredoField).GetAttribute("value") ?? "";
        }

}

