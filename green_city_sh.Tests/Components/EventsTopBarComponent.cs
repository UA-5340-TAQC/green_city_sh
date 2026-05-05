using OpenQA.Selenium;
using Allure.Net.Commons.Attributes;

namespace green_city_sh.Tests.Components;

public class EventsTopBarComponent : BaseComponent
{
    private By TopBarTitle => By.CssSelector(".main-header"); //повертає назву сторіни
    private By SearchIcon => By.XPath(".//span[@class='search-img']/.."); //повертає іконку пошуку (клік по якій відкриває поле для введення тексту пошуку)
    private By SearchInputField => By.CssSelector(".place-input"); //повертає поле для введення тексту пошуку (показується після кліку по іконці пошуку)
    private By BookmarkIcon => By.XPath(".//span[@class='bookmark-img']/.."); //повертає іконку закладок (клік по якій відкриває список збережених заходів)
    private By MyEventsIcon => By.XPath(".//img[@class='my-events-img']/.."); //повертає іконку "My events" (ПЕРЕВІРИТИ ФУНКЦІОНАЛ КНОПКИ)
    private By CreateEventButton => By.CssSelector(".create button"); //повертає кнопку "Create event" для створення нового заходу
    public EventsTopBarComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public EventsTopBarComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }

    public string GetTopBarTitle()
    {
        return "";
    }

    public void ClickSearchIcon() => WaitAndClick(SearchIcon);

    public void ClickBookmarkIcon()
    {
    }

    public void ClickMyEventsIcon()
    {
    }

    [AllureStep("Verify that the create event button is displayed")]
    public bool IsCreateEventButtonVisible()
    {
        return RootElement.FindElement(CreateEventButton).Displayed;
    }

    [AllureStep("Verify that the create event button is enabled")]
    public bool IsCreateEventButtonEnable()
    {
        return RootElement.FindElement(CreateEventButton).Enabled;
    }

    [AllureStep("Click the create event button")]

    public void ClickCreateEventButton()
    {
        RootElement.FindElement(CreateEventButton).Click();
    }

    public void FillSearchInputField(string searchText)
    {
        var searchInput = wait.Until(d =>
        {
            var element = RootElement.FindElement(SearchInputField);
            return element.Displayed ? element : null;
        });

        searchInput.Clear();
        searchInput.SendKeys(searchText);
        searchInput.SendKeys(Keys.Enter);
    }

    public bool IsSearchIconVisible()
    {
        return RootElement.FindElement(SearchIcon).Displayed;
    }

    public bool IsSearchIconEnabled()
    {
        var elements = RootElement.FindElements(SearchIcon);
        return elements.Count > 0 && elements[0].Enabled;
    }
}