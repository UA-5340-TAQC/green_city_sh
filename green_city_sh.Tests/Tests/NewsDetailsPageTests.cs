using System.Text.RegularExpressions;
using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using OpenQA.Selenium;

namespace green_city_sh.Tests.Tests;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class NewsDetailsPageTests : BaseTest
{
    private NewsDetailsPage? newsDetailsPage;
    private HeaderComponent? header;
    
    protected override void OnSetup()
    {
        Driver!.Manage().Window.Maximize();
        NavigateToBaseUrl();
        header = new HeaderComponent(Driver, Driver!.FindElement(By.CssSelector("header")));
        
        header.ChangeLanguage("En");
        header.ClickSignIn();
        var signInModal = SignInModalComponent.WaitAndCreate(Driver);
        
        signInModal.Login(Configuration.TestEmail, Configuration.TestPassword);
        newsDetailsPage = new NewsDetailsPage(Driver);
    }
    
    [Test]
    [Order(1)]
    [Description("Verify that the comments are displayed down the news page section")]
    [Category("Smoke")]
    public void VerifyCommentsCount()
    {
        Driver!.Navigate().GoToUrl(BaseUrl + "/news/10326");
        
        Assert.Multiple(() =>
        {
            Assert.That(newsDetailsPage.GetComments().Count, Is.EqualTo(7), "There should be 7 comments in the list");
            Assert.That(newsDetailsPage.GetReplies().Count, Is.EqualTo(3), "The comment should have 3 replies");
            Assert.That(newsDetailsPage.GetAllCommentsWithReplies().Count, Is.EqualTo(10), "The comment should have 11 comments including replies");
        });
    }

    [Test]
    [TestCase("Awesome")]
    [Order(2)]
    [Description("Verify that the user can delete their comment and the counter updates")]
    [Retry(1)]
    [Category("Comment")]
    public void VerifyDeletingUserCommentAndCounterUpdates(string comment)
    {
        Driver!.Navigate().GoToUrl(BaseUrl + "/news/10412");
        var initialCount = newsDetailsPage.WaitForCommentCounterVisible();
        newsDetailsPage.AddComment(comment);
        var afterAdd = newsDetailsPage.WaitForCommentCounterToChange(initialCount);
        
        newsDetailsPage
            .DeleteComment()
            .ClickCancelDelete();
        Assert.That(afterAdd, Is.EqualTo(initialCount + 1),
            "Counter should increase by 1 after adding comment");
        
        newsDetailsPage
            .DeleteComment()
            .ClickYesDelete();
        Driver.Navigate().Refresh();
        var afterDelete = newsDetailsPage.WaitForCommentCounterToChange(afterAdd);
        
        Assert.Multiple(() =>
        {
            Assert.That(newsDetailsPage.GetAllCommentsWithReplies().Count, Is.EqualTo(initialCount),
                "Comments list count should match initial after deletion");
            Assert.That(afterDelete, Is.EqualTo(initialCount),
                "Counter should return to initial value after deletion");
        });
    }

    public static IEnumerable<TestCaseData> EditTestDataConfig()
    {
        yield return new TestCaseData("Awesome", "Updated Comment");
        yield return new TestCaseData("Cool comment", "Updated Comment");
    }
    
    [Test]
    [TestCaseSource("EditTestDataConfig")]
    [Description("Verify that the user can successfully edit their own comment")]
    [Category ("Comment")]
    public void VerifyThatUserCanEditTheirOwnComment(string addText, string editText)
    {
        Driver!.Navigate().GoToUrl(BaseUrl + "/news/10412");
        
        newsDetailsPage
            .AddComment(addText)
            .EditComment(editText);
        
        Assert.That(newsDetailsPage.GetLastComment(), Is.EqualTo(editText), "The last comment should have the edited text");
        Assert.That(newsDetailsPage.IsEditedLabelDisplayed(), Is.True, "The label edited should be displayed after editing");
        newsDetailsPage
            .DeleteComment()
            .ClickYesDelete();
    }

    public static IEnumerable<TestCaseData> ReplyTestDataConfig()
    {
        yield return new TestCaseData("Awesome", "This is a reply");
    }
    
    [Test]
    [TestCaseSource("ReplyTestDataConfig")]
    [Category ("Comment")]
    [Description("Verify that the user can reply to another user's comment")]
    public void VerifyThatUserCanReplyToAnotherUserComment(string addText, string replyText)
    {
        Driver!.Navigate().GoToUrl(BaseUrl + "/news/10412");
        newsDetailsPage
            .AddComment(addText)
            .ReplyComment(replyText);
        try
        {
            Assert.Multiple(() =>
            {
                Assert.That(newsDetailsPage.GetLastReplyComment(), Is.EqualTo(replyText), 
                    "The last replied comment should match");
                Assert.That(newsDetailsPage.GetAttributeReplyButton.Contains("reply-active"), Is.True, 
                    "The reply button should be active");
                Assert.That(newsDetailsPage.IsHideReplyBtnDisplayed(), Is.True, "Hide reply button should be displayed");
            });

            newsDetailsPage
                .ClickHideReplies();
            Assert.That(newsDetailsPage.IsViewReplyBtnDisplayed, Is.True, "View reply button should be displayed");
            newsDetailsPage
                .ClickViewReplies();
        }
        finally
        {
            newsDetailsPage
                .DeleteComment()
                .ClickYesDelete();
        }
    }
    
    [Test]
    [Order(5)]
    [Description("Verify publication date is displayed in American format")]
    [Category ("Smoke")]
    public void VerifyPublicationDateIsDisplayedInAmericanFormat()
    {
        Driver!.Navigate().GoToUrl(BaseUrl + "/news/10412");
        
        var dateEn = newsDetailsPage.GetDateText();
        Assert.That(dateEn, Is.Not.Null.And.Not.Empty);
        
        var regexEn = new Regex(@"^[A-Z][a-z]{2} \d{2}, \d{4}$");
        Assert.That(regexEn.IsMatch(dateEn), Is.True, "Date should be localized in American");
    }
    
    [Test]
    [Order(6)]
    [Description("Verify publication date is displayed in Ukrainian format")]
    [Category ("Smoke")]
    public void VerifyPublicationDateIsDisplayedInUkrainianFormat()
    {
        header = new HeaderComponent(Driver, Driver.FindElement(By.CssSelector("header")));
        header.ChangeLanguage("Uk");
        Driver!.Navigate().GoToUrl(BaseUrl + "/news/10412");
        
        var dateUk = newsDetailsPage.GetDateText();
        Assert.That(dateUk, Is.Not.Null.And.Not.Empty);
        
        var regexUk = new Regex(@"^\d{2} [А-ЯҐЄІЇа-яґєії]+\. \d{4} р\.$");
        Assert.That(regexUk.IsMatch(dateUk), Is.True,
            "Date should be displayed in Ukrainian format");
    }
    
}
