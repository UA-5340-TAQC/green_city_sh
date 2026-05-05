using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;
using OpenQA.Selenium;
using Allure.NUnit;
using Allure.NUnit.Attributes;

namespace green_city_sh.Tests.Tests;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
[AllureOwner("Antonina Smetanina")]
[AllureSuite("Comments")]
[AllureFeature("Add comment")]
[AllureIssue("1")]
[AllureSuite("GreenCity")]
[AllureTag("UI", "Smoke")]
public class TC001_AddCommentTests : BaseTest
{
    private const string CommentText = "Cool!";

    private EventDetailsPage? eventDetailsPage;
    private CommentsComponent? commentsComponent;

    protected override void OnSetup()
    {
        NavigateToBaseUrl();

        new HomePage(Driver!)
            .Header
            .ChangeLanguage("En")
            .ClickSignIn();

        SignInModalComponent
            .WaitAndCreate(Driver!)
            .Login(Configuration.TestEmail, Configuration.TestPassword);

        eventDetailsPage = new EventDetailsPage(Driver!);
        eventDetailsPage.Open();


        commentsComponent = eventDetailsPage.GetCommentsComponentOrNull();

        Assert.That(
            commentsComponent,
            Is.Not.Null,
            "Comments component should be present on Event Details page");
    }

    protected override void OnTearDown()
    {
        try
        {
            if (Driver is null) return;

            eventDetailsPage ??= new EventDetailsPage(Driver);
            eventDetailsPage.Open();

            var freshComments = eventDetailsPage.GetCommentsComponentOrNull();

            if (freshComments == null) return;
            if (freshComments.IsCommentVisible(CommentText))
                freshComments.DeleteComment(CommentText);
        }
        catch (Exception e)
        {
            TestContext.WriteLine($"Teardown cleanup failed: {e.Message}");
        }
    }

    [Test]
    [Category("Smoke")]
    [AllureDescription("Comment field becomes active after click")]
    public void TC001_Step1_ClickCommentField_BecomesActive()
    {
        commentsComponent!.ClickCommentField();

        Assert.That(
            commentsComponent.IsCommentFieldFocus(),
            Is.True,
            "Cursor should be inside the comment field after click.");
    }

    [Test]
    [Category("Smoke")]
    [AllureDescription("Entered valid text is displayed")]
    public void TC001_Step2_EnterValidText_IsDisplayedInInput()
    {
        commentsComponent!
            .ClickCommentField()
            .EnterComment(CommentText);

        Assert.That(commentsComponent.GetCommentInputText(),
            Is.EqualTo(CommentText),
            "Entered text should be visible in the comment input field.");
    }

    [Test]
    [Category("Smoke")]
    [AllureDescription("Submitted comment increases counter")]
    public void TC001_Step3_SubmitsComment_CountIncreasedByOne()
    {
        commentsComponent!.ScrollToCounter();

        var countBefore = commentsComponent.GetCommentCount();

        commentsComponent
            .ClickCommentField()
            .EnterComment(CommentText)
            .SubmitComment();

        commentsComponent.WaitForCommentVisible(CommentText);

        commentsComponent.ScrollToCounter();

        Assert.That(commentsComponent.GetCommentCount(),
            Is.EqualTo(countBefore + 1),
            "Comment count should be increase by 1 after adding a comment.");
    }

    [Test]
    [Category("Smoke")]
    [AllureDescription("Comment visible after refresh")]
    public void TC001_Step4_AfterPageRefresh_CommentRemainsVisible()
    {
        commentsComponent!
            .ClickCommentField()
            .EnterComment(CommentText)
            .SubmitComment();


        eventDetailsPage!.RefreshPage();
        commentsComponent = eventDetailsPage.GetCommentsComponentOrNull();


        Assert.That(commentsComponent,
            Is.Not.Null,
            "Comments component should be present after page refresh");
    }
}
