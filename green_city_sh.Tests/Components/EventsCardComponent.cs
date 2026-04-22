using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class EventsCardComponent : BaseComponent
{
    // Верхня частина картки
    private By FavouriteButton => By.CssSelector(".favourite-button"); //повертає кнопку з "прапорцем" для додавання в обране
    private By RatingStars => By.CssSelector(".stars-filled"); //повертає кнопку з рейтингом у вигляді зірочок
    private By EventImage => By.CssSelector(".event-image"); //повертає зображення заходу на картці

    // Теги та основна інформація
    private By EventTags => By.CssSelector(".main-container .text"); //повертає тип заходу (ECONOMIC, ENVIRONMENTAL, SOCIAL)
    private By EventDate => By.CssSelector(".date-container .date"); //повертає дату проведення заходу
    private By EventTime => By.CssSelector(".date-container .time"); //повертає час проведення заходу
    private By EventLocation => By.CssSelector(".date-container p"); //повертає місце проведення заходу
    private By EventStatus => By.CssSelector(".event-status"); //повертає статус заходу (Open / Closed)
    private By EventTitle => By.CssSelector(".event-name"); //повертає назву заходу

    // Кнопки дій
    private By MoreButton => By.XPath(".//button[contains(text(), 'More')]"); //повертає кнопку "More" для відкриття додаткових опцій
    private By EditEventButton => By.XPath(".//button[contains(text(), 'Edit event')]"); //повертає кнопку "Edit event" для редагування заходу (показується лише для організатора)
    private By JoinEventButton => By.XPath(".//button[contains(text(), 'Join event')]"); //повертає кнопку "Join event" для приєднання до заходу (показується лише для користувачів, які не є учасниками)

    // Додаткова інформація (футер картки)
    private By CreatedDate => By.CssSelector(".date p"); //повертає дату створення заходу (показується лише для організатора та учасників)
    private By AuthorName => By.CssSelector(".author p"); //повертає ім'я автора заходу (показується лише для організатора та учасників)
    private By ParticipantsCount => By.CssSelector(".frame p"); //повертає кількість учасників заходу (показується лише для організатора та учасників)
    private By LikeButton => By.CssSelector(".like"); //повертає кнопку "Like", що дозволяє поставити лайк заходу (показується лише для організатора та учасників)
    private By DislikeButton => By.CssSelector(".dislike");  //повертає кнопку "Dislike", що дозволяє поставити дизлайк заходу (показується лише для організатора та учасників)
    private By LikeCount => By.CssSelector(".like span"); //повертає кількість лайків заходу (показується лише для організатора та учасників)
    private By DislikeCount => By.CssSelector(".dislike span"); //повертає кількість дизлайків заходу (показується лише для організатора та учасників)
    
    public EventsCardComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public EventsCardComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }

    public string GetEventTitle()
    {
        return RootElement.FindElement(EventTitle).Text.Trim();
    }

    public string GetEventDate()
    {
        return RootElement.FindElement(EventDate).Text.Trim();
    }

    public string GetEventTime()
    {
        return RootElement.FindElement(EventTime).Text.Trim();
    }

    public string GetEventLocation()
    {
        //Повернути місце проведення заходу
        return "";
    }

    public string GetEventStatus()
    {
        //Повернути статус заходу (Open/Closed)
        return "";
    }

    public List<string> GetEventTags()
    {
        //Повернути список тегів заходу (ECONOMIC, ENVIRONMENTAL, SOCIAL)
        return new List<string>();
    }

    public int GetParticipantsCount()
    {
        //Повернути кількість учасників
        return 0;
    }

    public int GetLikeCount()
    {
        //Повернути кількість лайків
        return 0;
    }

    public int GetDislikeCount()
    {
        //Повернути кількість дизлайків
        return 0;
    }

    public void ClickFavouriteButton()
    {
        RootElement.FindElement(FavouriteButton).Click();
    }

    public void ClickJoinButton()
    {
        //Приєднатися до заходу
    }

    public void ClickMoreButton()
    {
        //Відкрити додаткові опції
    }

    public void ClickLikeButton()
    {
        //Поставити лайк
    }

    public void ClickEditEventButton()
    {
        //Редагувати захід
    }

    public void ClickDislikeButton()
    {
        //Поставити дизлайк
    }

    public string GetAuthorName()
    {
        return RootElement.FindElement(AuthorName).Text.Trim();
    }

}