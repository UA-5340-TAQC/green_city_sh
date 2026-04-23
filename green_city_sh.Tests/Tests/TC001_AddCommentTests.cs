using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace green_city_sh.Tests.Tests;

[TestFixture]
public class TC001_AddCommentTests : BaseTest
{
    private const string TestEmail = "greencitytest69@hotmail.com";
    private const string TestPassword = "asweQA5346!)";
    private const string CommentText = "Cool!";

    private EventDetailsPage? eventDetailsPage;
    private CommentsComponent? commentsComponent;

    protected override void OnSetup()
    {
        NavigateToBaseUrl();
        var header = new HeaderComponent(Driver!, HeaderComponent.RootLocator);
        header.ChangeLanguage("En");

        header.ClickSignIn();
        SignInModalComponent
            .WaitAndCreate(Driver!)
            .Login(TestEmail, TestPassword);
        
        new WebDriverWait(Driver!, TimeSpan.FromSeconds(Configuration.DefaultTimeout))
            .Until(drv =>
                drv.FindElements(SignInModalComponent.RootLocator)
                    .All(e => !e.Displayed));

        eventDetailsPage = new EventDetailsPage(Driver!);
        eventDetailsPage.Open();

        commentsComponent = eventDetailsPage.GetCommentsComponent();
    }

    protected override void OnTearDown()
    {
        try
        {
            if (Driver is null) return;
            
            eventDetailsPage ??= new EventDetailsPage(Driver);
            eventDetailsPage.Open();
            
            var freshComments = eventDetailsPage.GetCommentsComponent();
            
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
    public void TC001_Step1_ClickCommentField_BecomesActive()
    {
        commentsComponent!.ClickCommentField();
    }

    [Test]
    [Category("Smoke")]
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
    public void TC001_Step3_SubmitsComment_CountIncreasedByOne()
    {
        new Actions(Driver!).ScrollToElement(
            Driver!.FindElement(By.CssSelector("div.counter"))
        ).Perform();

        var countBefore = commentsComponent!.GetCommentCount();
        
        commentsComponent
            .ClickCommentField()
            .EnterComment(CommentText)
            .SubmitComment();
        
        new WebDriverWait(Driver!, TimeSpan.FromSeconds(Configuration.DefaultTimeout))
            .Until(_ => commentsComponent.IsCommentVisible(CommentText));

        new WebDriverWait(Driver!, TimeSpan.FromSeconds(Configuration.DefaultTimeout))
            .Until(_ => commentsComponent.GetCommentCount() == countBefore + 1);
        
        Assert.That(commentsComponent.GetCommentCount(),
            Is.EqualTo(countBefore + 1),
            "Comment count should be increase by 1 after adding a comment.");
    }

    [Test]
    [Category("Smoke")]
    public void TC001_Step4_AfterPageRefresh_CommentRemainsVisible()
    {
        commentsComponent!
            .ClickCommentField()
            .EnterComment(CommentText)
            .SubmitComment();
        
        Driver!.Navigate().Refresh();
        commentsComponent = eventDetailsPage!.GetCommentsComponent();
        
        Assert.That(commentsComponent.IsCommentVisible(CommentText),
            Is.True,
            "Comment should persist after page refresh.");
    }
}
