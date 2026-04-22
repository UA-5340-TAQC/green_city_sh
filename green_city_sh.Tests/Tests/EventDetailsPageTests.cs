using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace green_city_sh.Tests.Tests;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class EventDetailsPageTests : BaseTest
{
    
    protected override void OnSetup()
    {
        Driver!.Manage().Window.Maximize();

        NavigateToBaseUrl();

        var header = new HeaderComponent(Driver, Driver!.FindElement(By.CssSelector("header")));
        
        header.ChangeLanguage("En");
        header.ClickSignIn();

        var signInModal = SignInModalComponent.WaitAndCreate(Driver);

        signInModal.Login("greencitytest69@hotmail.com", "asweQA5346!)");

        
    }

    [Test]
    [Order(1)]
    [Description("Verify that the user can save an event and view it in bookmarks")]
    [Retry(2)]
    [Category("Smoke")]
    public void VerifySavingEventAndViewingInBookmarks()
    {
        Driver!.Navigate().GoToUrl(BaseUrl + "/events/42");

        var eventDetailsCard = new EventDetailsCardComponent(Driver, Driver!.FindElement(By.CssSelector(".event-main")));
        var header = new HeaderComponent(Driver, Driver.FindElement(By.CssSelector("header")));
        var eventInfo = new EventInfoComponent(Driver, Driver.FindElement(By.CssSelector(".event")));
        

        string eventInfoText = eventInfo.GetEventInfo();
        

        Assert.IsFalse(eventDetailsCard.IsEventSaved(), "Event should not be saved initially.");

        eventDetailsCard.ClickSaveEvent();

        header.ClickBookmarks();

        var bookmarkTab = new BookmarkTabComponent(Driver, Driver.FindElement(By.CssSelector(".tabs")));
        bookmarkTab.SwitchToTab("Events");

        var eventsList = new EventsListComponent(Driver, Driver.FindElement(By.CssSelector(".event-list")));
        var savedEventCard = eventsList.GetSavedEventCard(eventInfoText);

        Assert.IsNotNull(savedEventCard, "Saved event should be visible in bookmarks.");

        savedEventCard.ClickFavouriteButton();
    }
}
