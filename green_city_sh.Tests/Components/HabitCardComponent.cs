using OpenQA.Selenium;

namespace green_city_sh.Tests.Components
{
    public class HabitCardComponent : BaseComponent
    {
        public HabitCardComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
        {
        }

        public HabitCardComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
        {
        }
    }
}