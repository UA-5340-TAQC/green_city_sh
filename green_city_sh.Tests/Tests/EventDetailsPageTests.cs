using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Modals;
using OpenQA.Selenium;

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

        signInModal.Login(Configuration.TestEmail, Configuration.TestPassword);
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

        Assert.IsTrue(eventDetailsCard.IsEventSaved(), "Event should be marked as saved after clicking save.");

        header.ClickBookmarks();

        Assert.That(Driver.Url, Is.EqualTo(BaseUrl + "/news?isBookmark=true"), "URL should navigate to bookmarks page.");

        var bookmarkTab = new BookmarkTabComponent(Driver, Driver.FindElement(By.CssSelector(".tabs")));
        bookmarkTab.SwitchToTab("Events");

        Assert.That(Driver.Url, Does.Contain("events"), "URL should contain 'events' after switching to Events tab.");

        var eventsList = new EventsListComponent(Driver, Driver.FindElement(By.CssSelector(".event-list")));
        var savedEventCard = eventsList.GetSavedEventCard(eventInfoText);

        Assert.IsNotNull(savedEventCard, "Saved event should be visible in bookmarks.");

        savedEventCard.ClickBookmark();
    }

    [Test]
    [Order(2)]
    [Description("Verify that the user cannot submit a comment with an uploaded image only")]
    [Retry(2)]
    [Category("Smoke")]
    public void VerifyCommentSubmissionWithImageOnly()
    {
        Driver!.Navigate().GoToUrl(BaseUrl + "/events/36");

        var commentComponent= new CommentComponent(Driver, Driver.FindElement(By.XPath("//app-comments-container[.//div[contains(@class, 'counter')]]")));

        Assert.IsTrue(commentComponent.IsCommentFieldEmpty(), "Comment field should be empty initially.");

        commentComponent.ClickUploadImgBtn();
        commentComponent.UploadImage("test_image.jpg");

        Assert.IsTrue(commentComponent.IsImagePreviewDisplayed(), "Image preview should be displayed after uploading an image.");

        Assert.IsTrue(commentComponent.IsCommentButtonDisabled(), "Submit button should be disabled when only an image is uploaded without text.");
    }

    [Test]
    [Order(3)]
    [Description("Verify that the user can cancel joining an event")]
    [Retry(2)]
    [Category("Smoke")]
    public void VerifyCancelJoiningEvent()
    {
        Driver!.Navigate().GoToUrl(BaseUrl + "/events/42");

        var eventDetailsCard = new EventDetailsCardComponent(Driver, Driver.FindElement(By.CssSelector(".event-main")));
        Assert.IsTrue(eventDetailsCard.IsJoinRequestCancelled(), "Join request should not be sent initially.");

        eventDetailsCard.ClickJoinEvent();
        Assert.IsTrue(eventDetailsCard.IsJoinRequestSent(), "Join request should be sent after clicking Join Event.");

        eventDetailsCard.ClickCancelRequest();

        var cancelModal = new CancelJoiningEventModal(Driver, Driver.FindElement(By.XPath(".//app-warning-pop-up[@class='ng-star-inserted']")));
        cancelModal.ClickYesButton();

        Assert.IsTrue(eventDetailsCard.IsJoinRequestCancelled(), "Join request should be cancelled after confirming cancellation.");

    }
}
