using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;

using Allure.NUnit;
using Allure.NUnit.Attributes;

namespace green_city_sh.Tests.Tests;

[TestFixture]
[AllureOwner("Antonina Smetanina")]
[AllureSuite("Create event")]
[AllureFeature("Publish button validation")]
public class TC010_PublishButton_RemainsDisabled : BaseTest
{
    private string TestEmail = null!;
    private string TestPassword = null!;

    private const string ValidTitle = "Test Event";
    private const string ValidDescription = "Test description for the event";
    private const string ShortDescription = "MyTesting";
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

        createEventPage = new CreateEventPage(Driver!);
        createEventPage.Open();
    }

    private void FillMandatoryFieldsExceptDescription()
    {
        createEventPage!
            .EnterTitle(ValidTitle)
            .SelectInvite()
            .EnterStartTimeInput(ValidStartTime)
            .EnterEndTimeInput(ValidEndTime)
            .ClickOnlineCheckbox()
            .EnterOnlineLink(ValidOnlineLink);
    }

    [Test]
    [Category("Smoke")]
    [AllureStory("Valid title is accepted")]
    public void TC010_Step1_EnterValidTitle_TitleIsAccepted()
    {
        createEventPage!.EnterTitle(ValidTitle);

        Assert.Pass("Valid title text was entered into the Title field.");
    }

    [Test]
    [Category("Smoke")]
    [AllureStory("Validation error for short description")]
    public void TC010_Step2_ValidErrorAppears()
    {
        createEventPage!.EnterTitle(ValidTitle);
        createEventPage.EnterDescription(ShortDescription);
        FillMandatoryFieldsExceptDescription();

        Assert.That(
            createEventPage.IsDescriptionErrorVisible(),
            Is.True,
            "Validation error should appear when description has less than 10 symbols.");
    }

    [Test]
    [Category("Smoke")]
    [AllureStory("Publish button remains disabled for invalid description")]
    public void TC010_Step3_PublishButton_RemainsDisabled()
    {
        createEventPage!.EnterTitle(ValidTitle);
        createEventPage.EnterDescription(ShortDescription);
        FillMandatoryFieldsExceptDescription();

        Assert.That(
            createEventPage.IsPublishButtonEnabled(),
            Is.False,
            "blish button should remain disabled when description is less than 10 symbols.");
    }
}
