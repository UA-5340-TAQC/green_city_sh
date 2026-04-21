using OpenQA.Selenium;

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
        //Реалізувати отримання тексту заголовка
        return "";
    }

    public void ClickSearchIcon()
    {
        //Реалізувати клік по іконці пошуку
    }

    public void ClickBookmarkIcon()
    {
        //Реалізувати клік по іконці закладок
    }

    public void ClickMyEventsIcon()
    {
        //Реалізувати клік по іконці "My events"
    }

    public void ClickCreateEventButton()
    {
        //Реалізувати клік по кнопці створення події
    }

    public void FillSearchInputField(string searchText)
    {
        //Реалізувати введення тексту в поле пошуку
    }
}