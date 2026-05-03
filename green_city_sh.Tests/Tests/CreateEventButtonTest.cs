using Allure.Net.Commons.Attributes;
using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;

namespace green_city_sh.Tests.Tests;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class CreateEventButtonTest : BaseTest
{
    private EventsPage? eventsPage;

    protected override void OnSetup()
    {
        NavigateToBaseUrl();

        eventsPage = new EventsPage(Driver!);
        
        eventsPage.Header.ChangeLanguage("en");
        eventsPage.Header.ClickSignIn();
        var signInModal = SignInModalComponent.WaitAndCreate(Driver!);
        signInModal.Login(Configuration.TestEmail, Configuration.TestPassword);

        eventsPage.OpenEventsPage();

    }

    [Test]
    [AllureIssue("36")]
    [AllureDescription("Verify that \"Create event\" button redirects to the event creation")]
    [Category("Smoke")]
    public void CreateEventTest()
    {
        Assert.That(eventsPage!.EventsTopBar.IsCreateEventButtonVisible(), "Create Event button should be visible for logged-in users");
        Assert.That(eventsPage!.EventsTopBar.IsCreateEventButtonEnable(), "Create Event button should be enabled");
        eventsPage!.ClickCreateEvent();
        Assert.That(Driver!.Url, Does.Contain("/events/create-update-event"), "URL should contain '/events/create' after clicking Create Event button");

    }
}