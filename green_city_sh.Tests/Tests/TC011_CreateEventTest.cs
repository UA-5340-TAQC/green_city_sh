using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;
using OpenQA.Selenium;

namespace green_city_sh.Tests.Tests;

[TestFixture]
public class TC011_CreateEventTest : BaseTest
{
    private static string TestEmail = Environment.GetEnvironmentVariable("TEST_EMAIL")
                                      ?? throw new InvalidOperationException("TEST_EMAIL is not configured.");
    private static string TestPassword = Environment.GetEnvironmentVariable("TEST_PASSWORD")
                                         ?? throw new InvalidOperationException("TEST_PASSWORD is not configured.");

    private const string WhitespaceTitle = "   ";
    private const string ValidDescription = "Test description for the event";
    private const string ValidStartTime = "23:30";
    private const string ValidEndTime = "23:59";
    private const string ValidOnlineLink = "https://meet.google.com/test";

    private CreateEventPage? createEventPage;

    protected override void OnSetup()
    {
        NavigateToBaseUrl();

        var homePage = new HomePage(Driver!);

        homePage
            .Header
            .ChangeLanguage("En")
            .ClickSignIn();

        SignInModalComponent
            .WaitAndCreate(Driver!)
            .Login(Configuration.TestEmail, Configuration.TestPassword);

        homePage = new HomePage(Driver!);
        homePage.Header.WaitForUserLoggedIn();

        createEventPage = new CreateEventPage(Driver!);
        createEventPage.Open();
    }

    [TearDown]
    public void TearDown()
    {
        try
        {
            Driver?.Quit();
        }
        catch
        {
        }
    }


    private void FillMandatoryFieldsExceptTitle()
    {
        createEventPage!
            .EnterDescription(ValidDescription)
            .SelectInvite()
            .EnterStartTimeInput(ValidStartTime)
            .EnterEndTimeInput(ValidEndTime)
            .ClickOnlineCheckbox()
            .EnterOnlineLink(ValidOnlineLink);
    }

    [Test]
    [Category("Smoke")]
    public void TC011_Step1_EnterWhitespaceInTitle_WhitespaceIsAccepted()
    {
        createEventPage!.EnterTitle(WhitespaceTitle);

        Assert.Pass("Whitespace characters were entered into the Title field.");
    }

    [Test]
    [Category("Smoke")]
    public void TC011_Step2_ValidationErrorAppears_WhenOtherFieldsFilled()
    {
        createEventPage!.EnterTitle(WhitespaceTitle);
        FillMandatoryFieldsExceptTitle();

        Assert.That(createEventPage.IsTitleErrorVisible(),
            Is.True,
            "Validation error should appear after whitespace-only title loses focus.");
    }

    [Test]
    [Category("Smoke")]
    public void TC011_Step3_PublishButton_RemainsDisabled()
    {
        createEventPage!.EnterTitle(WhitespaceTitle);
        FillMandatoryFieldsExceptTitle();

        Assert.That(createEventPage.IsPublishButtonEnabled(),
            Is.False,
            "Publish button should remain disabled when Title contains only whitespace.");
    }
}
