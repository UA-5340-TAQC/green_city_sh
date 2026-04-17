using OpenQA.Selenium;

namespace green_city_sh.Tests.Components
{
    public class NewsCardComponent : BaseComponent
    {
        public NewsCardComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
        {
        }

        public NewsCardComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
        {
        }
    }
}