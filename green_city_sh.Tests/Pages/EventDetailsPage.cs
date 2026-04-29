using OpenQA.Selenium;
using green_city_sh.Tests.Components;
using green_city_sh.Tests.Modals;

namespace green_city_sh.Tests.Pages;

public class EventDetailsPage : BasePage 
{
    private EventDetailsCardComponent? eventDetailsCard; 
    private CommentComponent? comments;
    private CancelJoiningEventModal? cancelModal;

    public EventDetailsPage(IWebDriver driver) : base(driver)
    {
    }

    public EventDetailsCardComponent EventDetailsCard => eventDetailsCard ??=
        new EventDetailsCardComponent(driver, By.CssSelector(".event"));

    public CommentComponent Comments => comments ??=
        new CommentComponent(driver, By.XPath("//app-comments-container[.//div[contains(@class, 'counter')]]"));

    public CancelJoiningEventModal CancelModal => cancelModal ??=
        new CancelJoiningEventModal(driver, By.XPath(".//app-warning-pop-up[@class='ng-star-inserted']"));
}