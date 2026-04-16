using OpenQA.Selenium;

namespace green_city_sh.Tests.Components
{
    public class MyNewsTabComponent : BaseComponent
    {
        public MyNewsTabComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
        {
        }

        public MyNewsTabComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
        {
        }
    }
}