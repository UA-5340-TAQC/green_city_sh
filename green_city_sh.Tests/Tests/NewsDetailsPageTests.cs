using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;
using OpenQA.Selenium;

namespace green_city_sh.Tests.Tests;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class NewsDetailsPageTests : BaseTest
{
    private NewsDetailsPage? _newsDetailsPage;

    [SetUp]
    public void SetUp()
    {
        Driver!.Manage().Window.Maximize();

        NavigateToBaseUrl();

        var header = new HeaderComponent(Driver, Driver!.FindElement(By.CssSelector("header")));
        
        header.ChangeLanguage("En");
        header.ClickSignIn();

        var signInModal = SignInModalComponent.WaitAndCreate(Driver);

        signInModal.Login("greencitytest69@hotmail.com", "asweQA5346!)");
        _newsDetailsPage = new NewsDetailsPage(Driver);
    }
    [Test]
    [Order(1)]
    [Category("Smoke")]
    public void VerifyCommentsCount()
    {
        Driver.Navigate().GoToUrl("https://www.greencity.cx.ua/#/greenCity/news/10326");
        NewsDetailsPage page = new NewsDetailsPage(Driver!);

        Assert.Multiple(() =>
        {
            Assert.That(page.GetComments().Count, Is.EqualTo(7), "There should be 7 comments in the list");
            Assert.That(page.GetReplies().Count, Is.EqualTo(2), "The comment should have 2 replies");
        });
        
    }

    [Test]
    [Order(1)]
    [Description("Verify that the user can delete their comment and the counter updates")]
    [Retry(1)]
    [Category("Smoke")]
    public void VerifyDeletingUserCommentAndCounterUpdates()
    {
        const string text = "Awesome";
        Driver!.Navigate().GoToUrl(BaseUrl + "/news/10412");
        var initialCount = _newsDetailsPage.GetCounterNumber();
        _newsDetailsPage.AddComment(text);
        _newsDetailsPage.
            DeleteComment()
            .ClickCancelDelete();
        Assert.That(_newsDetailsPage.GetCounterNumber(), Is.EqualTo(initialCount + 1));
        _newsDetailsPage
            .DeleteComment()
            .ClickYesDelete();
        Driver.Navigate().Refresh();
        Assert.Multiple(() =>
        {
            Assert.That(_newsDetailsPage.GetComments().Count, Is.EqualTo(initialCount), "Comments should remain the same amount");
            Assert.That(_newsDetailsPage.GetCounterNumber(), Is.EqualTo(initialCount), "Count should remain the same after deletion");
        });
        
    }

    [TearDown]
    public void TearDown()
    {
        base.TearDown();
    }

}