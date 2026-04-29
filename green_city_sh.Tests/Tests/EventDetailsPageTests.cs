using AngleSharp.Dom.Events;
using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Modals;
using green_city_sh.Tests.Pages;
using OpenQA.Selenium;

namespace green_city_sh.Tests.Tests;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class EventDetailsPageTests : BaseTest
{
    protected override void OnSetup()
    {
        NavigateToBaseUrl();

        HomePage homePage = new HomePage(Driver!);
        
        homePage.Header.ChangeLanguage("En");
        homePage.Header.ClickSignIn();

        var signInModal = SignInModalComponent.WaitAndCreate(Driver!);

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

        HomePage homePage = new HomePage(Driver!);

        EventDetailsPage eventDetailsPage = new EventDetailsPage(Driver!);

        string eventInfoText = eventDetailsPage.EventDetailsCard.GetEventInfo();
        
        Assert.IsFalse(eventDetailsPage.EventDetailsCard.IsEventSaved(), "Event should not be saved initially.");

        eventDetailsPage.EventDetailsCard.ClickSaveEvent();

        Assert.IsTrue(eventDetailsPage.EventDetailsCard.IsEventSaved(), "Event should be marked as saved after clicking save.");

        homePage.Header.ClickBookmarks();

        Assert.That(Driver!.Url, Is.EqualTo(BaseUrl + "/news?isBookmark=true"), "URL should navigate to bookmarks page.");

        SavedPage savedPage = new SavedPage(Driver!);

        savedPage.BookmarkTab.SwitchToTab("Events");

        Assert.That(Driver!.Url, Does.Contain("events"), "URL should contain 'events' after switching to Events tab.");

        var savedEventCard = savedPage.EventsList.GetSavedEventCard(eventInfoText);

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

        EventDetailsPage eventDetailsPage = new EventDetailsPage(Driver);

        Assert.IsTrue(eventDetailsPage.Comments.IsCommentFieldEmpty(), "Comment field should be empty initially.");

        eventDetailsPage.Comments.ClickUploadImgBtn();
        eventDetailsPage.Comments.UploadImage("test_image.jpg");

        Assert.IsTrue(eventDetailsPage.Comments.IsImagePreviewDisplayed(), "Image preview should be displayed after uploading an image.");

        Assert.IsTrue(eventDetailsPage.Comments.IsCommentButtonDisabled(), "Submit button should be disabled when only an image is uploaded without text.");
    }

    [Test]
    [Order(3)]
    [Description("Verify that the user can cancel joining an event")]
    [Retry(2)]
    [Category("Smoke")]
    public void VerifyCancelJoiningEvent()
    {
        Driver!.Navigate().GoToUrl(BaseUrl + "/events/42");

        EventDetailsPage eventDetailsPage = new EventDetailsPage(Driver);

        Assert.IsTrue(eventDetailsPage.EventDetailsCard.IsJoinRequestCancelled(), "Join request should not be sent initially.");

        eventDetailsPage.EventDetailsCard.ClickJoinEvent();
        Assert.IsTrue(eventDetailsPage.EventDetailsCard.IsJoinRequestSent(), "Join request should be sent after clicking Join Event.");

        eventDetailsPage.EventDetailsCard.ClickCancelRequest();

        eventDetailsPage.CancelModal.ClickYesButton();

        Assert.IsTrue(eventDetailsPage.EventDetailsCard.IsJoinRequestCancelled(), "Join request should be cancelled after confirming cancellation.");

    }
}
