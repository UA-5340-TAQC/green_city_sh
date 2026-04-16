using OpenQA.Selenium;

namespace green_city_sh.Tests.Components
{
    public class MyHabitsTabComponent : BaseComponent
    {
        public MyHabitsTabComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
        {
        }

        public MyHabitsTabComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
        {
        }
    }
}