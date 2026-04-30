using AngleSharp.Dom.Events;
using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Modals;
using green_city_sh.Tests.Pages;
using OpenQA.Selenium;

namespace green_city_sh.Tests.Tests;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class EventPageTests : BaseTest
{
    protected override void OnSetup()
    {
        NavigateToBaseUrl();

        HomePage homePage = new HomePage(Driver!);
        
        homePage.Header.ChangeLanguage("En");
    }

    [Test]
    [Order(1)]
    [Description("Verify that the user can save an event and view it in bookmarks")]
    [Retry(2)]
    [Category("Smoke")]
    public void VerifyGuestRestrictionsOnEventPage()
    {
        Driver!.Navigate().GoToUrl(BaseUrl + "/events");

        EventsPage eventsPage = new EventsPage(Driver);
        var eventCard = eventsPage.EventList.GetEventCardByIndex(1);

        eventCard.ClickMore();

        Assert.That(Driver!.Url, Is.Not.EqualTo(BaseUrl + "/events"));

        EventDetailsPage eventDetailsPage = new EventDetailsPage(Driver);
        eventDetailsPage.EventDetailsCard.ClickJoinEvent();

        var signInModal = SignInModalComponent.WaitAndCreate(Driver);
        signInModal.CloseModal();

        Assert.IsTrue(eventDetailsPage.EventDetailsCard.IsJoinEventButtonVisible(), "Join button should be visible after closing sign-in modal.");

        eventDetailsPage.EventDetailsCard.ClickBackToEvents();

        Assert.That(Driver!.Url, Is.EqualTo(BaseUrl + "/events"));
    }
}