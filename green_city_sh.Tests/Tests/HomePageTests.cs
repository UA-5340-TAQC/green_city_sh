using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;

namespace green_city_sh.Tests.Tests;

[TestFixture]
public class HomePageTests : BaseTest
{
    private HomePage? _homePage;

    protected override void OnSetup()
    {
        _homePage = new HomePage(Driver!);
        _homePage.Open(BaseUrl);
    }

    [Test]
    [Category("Smoke")]
    public void VerifyHomePageLoads()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_homePage!.GetTitle(), Is.Not.Empty, "Page title should not be empty");
            Assert.That(_homePage!.GetTitle(), Is.EqualTo("GreenCity — Build Eco-Friendly Habits Today"), "Page title should match the expected value");
        });
    }

    [Test]
    public void VerifyCommentsCount()
    {
        Driver.Navigate().GoToUrl("https://www.greencity.cx.ua/#/greenCity/news/10326");
        NewsDetailsPage page = new NewsDetailsPage(Driver!);

        Assert.Multiple(() =>
        {
            //Assert.That(page.GetCommentsCount(), Is.EqualTo(7), "Comments count should be 7");
            Assert.That(page.GetComments().Count, Is.EqualTo(7), "There should be 7 comments in the list");
        });
        Assert.That(page.GetReplies().Count, Is.EqualTo(2), "The comment should have 2 replies");
    }
    
     [TearDown]
     public void TearDown()
     {
         Driver?.Quit();
    }
}
