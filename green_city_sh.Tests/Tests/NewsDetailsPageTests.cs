using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;

namespace green_city_sh.Tests.Tests;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class NewsDetailsPageTests : BaseTest
{
    private NewsDetailsPage? _newsDetailsPage;

    [SetUp]
    public void SetUp()
    {
        _newsDetailsPage = new NewsDetailsPage(Driver!);
        _newsDetailsPage.OpenNewsDetailsPage(51);
    }

    [Test]
    [Order(1)]
    [Description("Verify that the user can delete their comment and the counter updates")]
    [Retry(2)]
    [Category("Smoke")]
    public void VerifyDeletingUserCommentAndCounterUpdates()
    {
        _newsDetailsPage!.AddComment("Awesome");
        var initialCount = _newsDetailsPage.GetCommentsCount();
        _newsDetailsPage
            .DeleteComment()
            .ClickCancelDelete();
        var afterCancel = _newsDetailsPage.GetCommentsCount();
        Assert.That(afterCancel, Is.EqualTo(initialCount));
        _newsDetailsPage
            .DeleteComment()
            .ClickYesDelete();
        var afterDelete = _newsDetailsPage.GetCommentsCount();
        Driver!.Navigate().Refresh();
        Assert.That(afterDelete, Is.EqualTo(initialCount - 1),
            "Comment count should decrease by 1 after deletion");
    }

    [TearDown]
    public void TearDown()
    {
        Driver?.Quit();
    }

}