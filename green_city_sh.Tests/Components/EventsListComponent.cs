using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

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
        return driver.FindElements(AllEventCards).Count;
    }
    
    public List<EventsCardComponent> GetAllEventCards()
    {
        try
        {
            var elements = driver.FindElements(AllEventCards);

            if (elements.Count == 0)
            {
            return new List<EventsCardComponent> ();
            }
            return elements.Select(e => new EventsCardComponent(driver, e)).ToList();
        }
        catch (StaleElementReferenceException)
        {
            var elementsRetry = driver.FindElements(AllEventCards);
            return elementsRetry.Select(e => new EventsCardComponent(driver, e)).ToList();
        }
    }
    

    public EventsCardComponent GetEventCardByIndex(int index)
    {
        //Повернути картку події за індексом
        IWebElement eventCardRoot = RootElement.FindElement(EventCardByIndex(index));
        return new EventsCardComponent(driver, eventCardRoot);
    }

    public bool IsEndPageMessageDisplayed()
    {
        //Перевірити, чи відображається повідомлення "You have reached the end of the page"
        var endPageMessages = driver.FindElements(EndPageMessage);
        return endPageMessages.Count > 0 && endPageMessages[0].Displayed;
    }
}
