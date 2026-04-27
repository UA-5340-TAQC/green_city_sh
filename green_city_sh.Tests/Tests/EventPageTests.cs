using AngleSharp.Dom.Events;
using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Modals;
using OpenQA.Selenium;

namespace green_city_sh.Tests.Tests;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class EventPageTests : BaseTest
{
    protected override void OnSetup()
    {
        Driver!.Manage().Window.Maximize();

        NavigateToBaseUrl();

        var header = new HeaderComponent(Driver, Driver!.FindElement(By.CssSelector("header")));
        
        header.ChangeLanguage("En");
    }

    [Test]
    [Order(1)]
    [Description("Verify that the user can save an event and view it in bookmarks")]
    [Retry(2)]
    [Category("Smoke")]
    public void VerifyGuestRestrictionsOnEventPage()
    {
        Driver!.Navigate().GoToUrl(BaseUrl + "/events");

        var eventList = new EventsListComponent(Driver, Driver.FindElement(By.CssSelector(".event-list")));
        var eventCard = eventList.GetEventCardByIndex(1);

        eventCard.ClickMoreButton();

        Assert.That(Driver!.Url, Is.Not.EqualTo(BaseUrl + "/events"));

        var eventDetailsCard = new EventDetailsCardComponent(Driver, Driver.FindElement(By.XPath(".//div[@class='event ng-star-inserted']")));
        eventDetailsCard.ClickJoinEvent();

        var signInModal = SignInModalComponent.WaitAndCreate(Driver);
        signInModal.CloseModal();

        Assert.IsTrue(eventDetailsCard.IsJoinEventButtonVisible(), "Join button should be visible after closing sign-in modal.");

        eventDetailsCard.ClickBackToEvents();

        Assert.That(Driver!.Url, Is.EqualTo(BaseUrl + "/events"));
    }
}