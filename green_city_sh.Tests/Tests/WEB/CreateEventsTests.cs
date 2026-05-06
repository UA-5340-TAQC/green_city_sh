using System;
using System.Globalization;
using NUnit.Framework;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;
using green_city_sh.Tests.Components;
using Allure.NUnit.Attributes;

namespace green_city_sh.Tests.Tests.WEB;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class CreateEventsTests : BaseTest
{
    // --- Auth Data ---
    private static string GetRequiredEnv(string key)
    {
        var value = Environment.GetEnvironmentVariable(key);
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"Environment variable '{key}' is missing or empty.");
        }
        return value;
    }

    // Dynamically generating tomorrow's date formatted to exactly match the expected input
    string dynamicDateValue = DateTime.Today.AddDays(1).ToString("MMMM d, yyyy", new CultureInfo("en-US"));

    // Evaluated at runtime using '=>', guaranteeing .env is loaded first
    private static string TestEmail => GetRequiredEnv("GC_TEST_EMAIL");
    private static string TestPassword => GetRequiredEnv("GC_TEST_PASSWORD");

    // --- Test Data & Constants ---
    private const string EventTitle = "Eco Meetup 2026";
    private const string InitiativeTypeValue = "Environmental";
    private const string EventTypeValue = "Open";
    private const string InviteValue = "All";
    private const string DescriptionValue = "Let's discuss how to save our planet together";
    private const string StartTimeValue = "16:00";
    private const string EndTimeValue = "20:30";
    private const string OnlineLinkValue = "https://duckduckgo.com";
    private const string ExpectedCounterValue = "2/5";
    private const string ExpectedEventsUrlSubstring = "/events";

    private CreateUpdateEventPage _eventPage = null!;

    private const string EventTitle1 = "Green City Cleanup Festival 2026";
    private const string InitiativeTypeValue1 = "Social";
    private const string DescriptionValue1 = "Join us to clean the park and make our city greener!";
    private const string AddressValue1 = "Mitskevycha Square, 14, Ivano-Frankivs'k";

    protected override void OnSetup()
    {
        _eventPage = new CreateUpdateEventPage(Driver!);
        PerformLogin(_eventPage);
    }

    private void PerformLogin(CreateUpdateEventPage page)
    {
        NavigateToBaseUrl();

        // Wait for and click the Header Sign In button using encapsulated wait logic
        page.ClickHeaderSignInButton();

        // Instantiate the modal using the static factory method
        var signInModal = SignInModalComponent.WaitAndCreate(Driver!);

        signInModal.Login(Configuration.TestEmail, Configuration.TestPassword);

        // Wait for the modal to disappear using encapsulated wait logic
        page.WaitForLoginModalToDisappear();
    }

    [Test]
    [AllureIssue("15")]
    [AllureDescription("Verification that a user can create a new offline event with a physical address.")]
    [Category("Smoke")]
    public void VerifyUserCanCreateNewEvent()
    {
        // --- Arrange ---
        _eventPage.NavigateToCreateEventPageFromProfile();

        // --- Act ---
        _eventPage.SetTitle(EventTitle1);
        _eventPage.SelectInitiativeType(InitiativeTypeValue1);
        _eventPage.SelectEventType(EventTypeValue);
        _eventPage.SetDescription(DescriptionValue1);

        _eventPage.SetDate(dynamicDateValue);
        _eventPage.SetTime("23:30", "23:59");

        _eventPage.SelectPlaceLocation();
        _eventPage.EnterAddress(AddressValue1);
        _eventPage.SelectAddressFromDropdown();

        _eventPage.ClickPreview();
        _eventPage.ClickBackToEditing();

        var prePublishUrl = Driver!.Url;
        _eventPage.ClickPublish();

        // Wait for the redirection to execute fully using encapsulated wait
        bool isRedirected = _eventPage.WaitForUrlToChange(prePublishUrl) &&
                            _eventPage.WaitForUrlToContain("/events");

        // --- Assert ---
        Assert.That(isRedirected, Is.True, "Redirection failed: User was not redirected to the events page after publishing.");
    }

    [Test]
    [AllureIssue("9")]
    [AllureDescription("Verification of creating a 1-day online event with all mandatory fields and an uploaded picture.")]
    [Category("CreateEvent")]
    [Description("TC-009: Successful creation of a 1-day online event with all mandatory fields and a picture")]
    public void CreateOneDayOnlineEvent_WithMandatoryFieldsAndPicture_Successfully()
    {
        // --- Arrange ---

        _eventPage.NavigateToCreateEventPageFromProfile();

        // --- Act ---
        _eventPage.SetTitle(EventTitle);
        _eventPage.SelectInitiativeType(InitiativeTypeValue);
        _eventPage.SelectEventType(EventTypeValue);
        _eventPage.SelectInviteOption(InviteValue);
        _eventPage.SetDescription(DescriptionValue);

        _eventPage.SetDate(dynamicDateValue);
        _eventPage.SetStartTime(StartTimeValue);
        _eventPage.SetEndTime(EndTimeValue);

        _eventPage.CheckOnlineLocation();
        _eventPage.SetOnlineLink(OnlineLinkValue);
        _eventPage.CheckApplyToAllDays();

        _eventPage.ScrollToPictureSection();
        _eventPage.SelectGreencityPicture(1);

        // --- Assert Intermediate Business Logic (Image Upload States) ---
        Assert.Multiple(() =>
        {
            Assert.That(_eventPage.IsUploadedImagePreviewDisplayed(), Is.True,
                "Failed: The image was not added to the upload block.");
            Assert.That(_eventPage.IsMainBadgeDisplayed(), Is.True,
                "Failed: The image did not receive the 'Main' badge.");
            Assert.That(_eventPage.IsClosePictureIconDisplayed(), Is.True,
                "Failed: The 'X' (close) button is not displayed on the image.");
            Assert.That(_eventPage.GetPictureCounterText(), Does.Contain(ExpectedCounterValue),
                $"Failed: The counter did not update to '{ExpectedCounterValue}'.");
        });

        // --- Act & Assert Readiness ---
        _eventPage.HoverOverPublishButton();
        Assert.That(_eventPage.IsPublishButtonEnabled(), Is.True,
            "Failed: The 'Publish' button did not become active.");

        // --- Final Submission ---
        var prePublishUrl = Driver!.Url;
        _eventPage.ClickPublish();

        // Properly wait for the URL to change first, THEN ensure it contains the expected substring
        bool isRedirected = _eventPage.WaitForUrlToChange(prePublishUrl) &&
                            _eventPage.WaitForUrlToContain(ExpectedEventsUrlSubstring);

        // --- Final Assertions ---
        Assert.Multiple(() =>
        {
            Assert.That(isRedirected, Is.True,
                "Step 16 Failed: The user was not redirected to the Events page within the given timeout period.");

            // Explicitly verify the URL is no longer the pre-publish URL
            Assert.That(Driver!.Url, Is.Not.EqualTo(prePublishUrl).IgnoreCase,
                "Step 16 Failed: The URL did not change after clicking Publish.");

            Assert.That(_eventPage.IsSuccessSnackBarDisplayed(), Is.True,
                "Step 16 Failed: The success message toast is not displayed.");

            Assert.That(Driver!.Url, Does.Contain(ExpectedEventsUrlSubstring),
                "Step 16 Failed: The URL does not contain the expected events substring.");
        });
    }

    [TearDown]
    public void LocalTearDown()
    {
        // Postconditions: Delete created event
    }
}