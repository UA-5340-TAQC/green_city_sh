using OpenQA.Selenium;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;
using System;
using Allure.NUnit.Attributes;

namespace green_city_sh.Tests.Components
{
    public class NewsCardComponent : BaseComponent
    {
        private By TagsInCard => By.CssSelector(".filter-tag .ul-eco-buttons span");
        private By TitleLocator => By.CssSelector(".title-list h3");
        private By BookmarkIconLocator => By.CssSelector(".favourite-button");
        //span.bookmark-img

        public NewsCardComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
        {
        }

        public NewsCardComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
        {
        }

        [AllureStep("Get tags from news card")]
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

        [AllureStep("Get title from news card")]
        public string GetTitle()
        {
            return RootElement.FindElement(TitleLocator).Text.Trim();
        }
        
        [AllureStep("Check if news card is bookmarked")]
        public bool IsBookmarked()
        {
            var icon = RootElement.FindElement(BookmarkIconLocator);
            return icon.GetAttribute("class").Contains("active");
        }

        [AllureStep("Click bookmark icon")]
        public void ClickBookmark()
        {
            var bookmarkBtn = RootElement.FindElement(BookmarkIconLocator);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].click();", bookmarkBtn);
        }

    }
}
