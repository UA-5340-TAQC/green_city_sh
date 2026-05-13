using Allure.NUnit.Attributes;
using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;


namespace green_city_sh.Tests.Tests.WEB;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
[AllureOwner("Antonina Smetanina")]
[AllureSubSuite("Create event")]
[AllureFeature("Title validation")]
[AllureIssue("12")]
[AllureTag("UI", "Smoke")]
public class TC011_CreateEventTest : BaseUITest
{
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
    public override void TearDown()
    {
        base.OnTearDown();
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
    [AllureDescription("Inability to save an event with a whitespace value in 'Title' field")]
    public void TC011_Step1_EnterWhitespaceInTitle_WhitespaceIsAccepted()
    {
        createEventPage!.EnterTitle(WhitespaceTitle);

        Assert.Pass("Whitespace characters were entered into the Title field.");
    }

    [Test]
    [Category("Smoke")]
    [Ignore("TEMP: Publish button is disabled due to ongoing feature improvements. " +
            "Enable after Publish becomes active.")]

    [AllureDescription("Title validation error, when other fields filled")]
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
    [AllureDescription("Title validation")]
    public void TC011_Step3_PublishButton_RemainsDisabled()
    {
        createEventPage!.EnterTitle(WhitespaceTitle);
        FillMandatoryFieldsExceptTitle();

        Assert.That(createEventPage.IsPublishButtonEnabled(),
            Is.False,
            "Publish button should remain disabled when Title contains only whitespace.");
    }
}
