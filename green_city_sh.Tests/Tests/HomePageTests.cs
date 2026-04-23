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
        var pageTitle = _homePage!.getTitle();

        Assert.Multiple(() =>
        {
            Assert.That(pageTitle, Is.Not.Empty, "Page title should not be empty");
            Assert.That(pageTitle, Does.Contain("GreenCity"), "Page title should contain 'GreenCity'");
        });
    }
}


