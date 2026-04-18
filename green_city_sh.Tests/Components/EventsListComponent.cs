using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class EventsListComponent : BaseComponent
{
    private By AllEventCards => By.CssSelector(".event-list-item"); //повертає всі картки заходів, які відображаються на сторінці

    private By EventCardByIndex(int index) => By.CssSelector($".event-list-item:nth-of-type({index + 1})"); //повертає картку заходу за індексом

    private By EndPageMessage => By.CssSelector(".end-page-txt"); //повертає повідомлення "You have reached the end of the page", яке відображається, коли користувач прокрутив всі заходи на сторінці

    public EventsListComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public EventsListComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }

    public int GetEventCardsCount()
    {
        //Повернути кількість карток подій на сторінці
        return 1;
    }

    public List<EventsCardComponent> GetAllEventCards()
    {
        //Повернути список всіх карток подій
        return new List<EventsCardComponent>();
    }

    public EventsCardComponent GetEventCardByIndex(int index)
    {
        //Повернути картку події за індексом
        return new EventsCardComponent(driver, EventCardByIndex(index));
    }

    public bool IsEndPageMessageDisplayed()
    {
        //Перевірити, чи відображається повідомлення "There are no more events for now"
        return true;
    }
}