using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace green_city_sh.Tests.Components;

public class EventsListComponent : BaseComponent
{
    private By AllEventCards => By.CssSelector(".event-list-item"); //повертає всі картки заходів, які відображаються на сторінці

    private By EventCardByIndex(int index) => By.CssSelector($".event-list-item:nth-of-type({index + 1})"); //повертає картку заходу за індексом

    private By EndPageMessage => By.CssSelector(".end-page-txt"); //повертає повідомлення "You have reached the end of the page", яке відображається, коли користувач прокрутив всі заходи на сторінці

    private By LoadingSpinner => By.CssSelector(".mdc-circular-progress__spinner-layer");

    public EventsListComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public EventsListComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }

    public int GetEventCardsCount()
    {
        return driver.FindElements(AllEventCards).Count;
    }

    public void WaitForCardsToLoad()
    {
        wait.Until(ExpectedConditions.InvisibilityOfElementLocated(LoadingSpinner));
    }

    public List<EventCardComponent> GetAllEventCards()
    {
        var eventCards = new List<EventCardComponent>();
        var eventCardsCount = GetEventCardsCount();

        for (int i = 0; i < eventCardsCount; i++)
        {
            eventCards.Add(GetEventCardByIndex(i));
        }

        return eventCards;
    }

    public EventCardComponent GetEventCardByIndex(int index)
    {
        IWebElement eventCardRoot = RootElement.FindElement(EventCardByIndex(index));
        return new EventCardComponent(driver, eventCardRoot);
    }

    public bool IsEndPageMessageDisplayed()
    {
        var endPageMessages = driver.FindElements(EndPageMessage);
        return endPageMessages.Count > 0 && endPageMessages[0].Displayed;
    }
}
