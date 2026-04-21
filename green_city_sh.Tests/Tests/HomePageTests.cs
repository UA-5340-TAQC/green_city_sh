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
}
