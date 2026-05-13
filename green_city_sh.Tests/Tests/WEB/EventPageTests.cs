using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;
using Allure.Net.Commons.Attributes;
using Allure.Net.Commons;



namespace green_city_sh.Tests.Tests.WEB;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
[AllureOwner("Petro Derlytsia")]
[AllureFeature("Event Page")]
public class EventPageTests : BaseUITest
{
    protected override void OnSetup()
    {
        NavigateToBaseUrl();

        HomePage homePage = new HomePage(Driver!);

        homePage.Header.ChangeLanguage("En");
    }

    [Test]
    [Order(1)]
    [AllureDescription("Verify guest restrictions on Event page")]
    [Retry(2)]
    [Category("Regression")]
    [Category("Security")]
    [Category("Negative")]
    [AllureIssue("17")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureSuite("GreenCity")]
    [AllureSubSuite("Format")]
    [AllureTag("UI", "Sanity")]
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
