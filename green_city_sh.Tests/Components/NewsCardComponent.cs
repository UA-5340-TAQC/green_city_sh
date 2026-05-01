using OpenQA.Selenium;

namespace green_city_sh.Tests.Components
{
    public class NewsCardComponent : BaseComponent
    {
        private By TagsInCard => By.CssSelector(".filter-tag .ul-eco-buttons span");

        public NewsCardComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
        {
        }

        public NewsCardComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
        {
        }

        /// <summary>
        /// Returns a list of tags for the current news card.
        /// </summary>
        public List<string> GetTags()
        {
            var tagElements = RootElement.FindElements(TagsInCard);
            var tags = new List<string>();

            foreach (var element in tagElements)
            {
                var text = element.Text.Trim();
                if (!string.IsNullOrEmpty(text) && text != "|")
                {
                    tags.Add(text);
                }
            }

            return tags;
        }
    }
}
