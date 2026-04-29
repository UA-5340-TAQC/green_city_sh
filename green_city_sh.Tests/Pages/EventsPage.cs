using OpenQA.Selenium;
using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;

namespace green_city_sh.Tests.Pages;

public class EventsPage : BasePage
{
    private EventsTopBarComponent? eventsTopBar;
    private EventsFilterSectionComponent? filterSection;
    private EventsListComponent? eventList;

    public EventsTopBarComponent EventsTopBar => eventsTopBar ??= new EventsTopBarComponent(driver, By.CssSelector(".event-header"));    
    public EventsFilterSectionComponent FilterSection => filterSection ??= new EventsFilterSectionComponent(driver, By.CssSelector(".filter-container"));
    public EventsListComponent EventList => eventList ??= new EventsListComponent(driver, By.CssSelector(".event-list"));

    private By ItemsFoundText => By.CssSelector(".active-filter-container p"); //повертає текст, який відображає кількість знайдених заходів після застосування фільтрів (наприклад, "5 items found")
    private By GalleryViewButton => By.CssSelector(".gallery"); //повертає кнопку для перемикання на галерею (клік по якій змінює відображення заходів з списку на галерею)
    private By ListViewButton => By.CssSelector(".list"); //повертає кнопку для перемикання на список (клік по якій змінює відображення заходів з галереї на список)

    public EventsPage(IWebDriver driver) : base(driver)
    {
    }

    //Перед відкриттям сторінки користувач має бути залогінений

    public void OpenEventsPage()
    {
        string baseUrl = Configuration.BaseUrl.TrimEnd('/');
        string eventsUrl = $"{baseUrl}/events";
        driver.Navigate().GoToUrl(eventsUrl);
    }

    public string GetItemsFoundText()
    {
        //Повернути текст з кількістю знайдених заходів (наприклад, "5 items found")
        return "";
    }

    public int GetItemsFoundCount()
    {
        //Парсити текст і повернути кількість (наприклад,  з рядка "5 items found" повернути 5)
        return 0;
    }

    public void SwitchToGalleryView()
    {
        //Перемкнути відображення на галерею
    }

    public void SwitchToListView()
    {
        //Перемкнути відображення на список
    }

    public bool IsGalleryViewActive()
    {
        //Перевірити, чи активний gallery view
        return true;
    }

    public bool IsListViewActive()
    {
        //Перевірити, чи активний list view (до елемента картки додається клас .list-view)
        return driver.FindElements(By.CssSelector(".list-view")).Count > 0;
    }

    public void ClickSearchButton()
    {
        EventsTopBar.ClickSearchIcon();
    }

    public void ApplyFilter(string category, string value)
    {
        //Застосувати фільтр за категорією та значенням
    }

    public void ResetAllFilters()
    {
        //Скинути всі фільтри
    }

    public void ClickCreateEvent()
    {
        //Клікнути на кнопку створення події
    }

    public void JoinEventByIndex(int index)
    {
        //Приєднатися до заходу за індексом в списку
    }

    public EventCardComponent GetEventCardByIndex(int index)
    {
        //Повернути картку заходу за індексом
        return EventList.GetEventCardByIndex(index);
    }

    public List<EventsCardComponent> GetAllEventCards()
    {
        //Повернути всі картки заходів
        return EventList.GetAllEventCards();
    }
}
