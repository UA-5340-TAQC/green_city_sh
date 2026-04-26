using System;
using System.Globalization;
using NUnit.Framework;
using OpenQA.Selenium.Interactions;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;
using green_city_sh.Tests.Components;

namespace green_city_sh.Tests.Tests;

[TestFixture]
public class CreateEventTests : BaseTest
{
    // --- Auth Data ---
    private static readonly string TestEmail = Environment.GetEnvironmentVariable("GC_TEST_EMAIL") 
        ?? throw new InvalidOperationException("GC_TEST_EMAIL environment variable is missing.");
    private static readonly string TestPassword = Environment.GetEnvironmentVariable("GC_TEST_PASSWORD") 
        ?? throw new InvalidOperationException("GC_TEST_PASSWORD environment variable is missing.");

    // --- Test Data & Constants ---
    private const string EventTitle = "Eco Meetup 2026";
    private const string ExpectedDefaultDuration = "1 day";
    private const string InitiativeTypeValue = "Environmental";
    private const string EventTypeValue = "Open";
    private const string InviteValue = "All";
    private const string DescriptionValue = "Let's discuss how to save our planet together";
    private const string StartTimeValue = "16:00";
    private const string EndTimeValue = "20:30";
    private const string OnlineLinkValue = "https://duckduckgo.com";
    private const string ExpectedCounterValue = "1/5";
    private const string ExpectedEventsUrlSubstring = "/events";

    private void PerformLogin(CreateUpdateEventPage page)
    {
        NavigateToBaseUrl();

        // 1. Wait and click the Header Sign In button using encapsulated wait logic
        page.ClickHeaderSignInButton();

        // 2. Instantiate the modal using the static factory method
        var signInModal = SignInModalComponent.WaitAndCreate(Driver!);

        // 3. Perform login
        signInModal.Login(TestEmail, TestPassword);

        // 4. Wait for the modal to disappear using encapsulated wait logic
        page.WaitForLoginModalToDisappear();
    }

    [Test]
    [Category("CreateEvent")][Description("TC-009: Successful creation of a 1-day online event with all mandatory fields and a picture")]
    public void CreateOneDayOnlineEvent_WithMandatoryFieldsAndPicture_Successfully()
    {
        // --- Arrange ---
        // Dynamically generating tomorrow's date formatted to exactly match the expected input (e.g. "April 26, 2026")
        string dynamicDateValue = DateTime.Today.AddDays(1).ToString("MMMM d, yyyy", new CultureInfo("en-US"));
        
        var eventPage = new CreateUpdateEventPage(Driver!);
        var actions = new Actions(Driver!);

        // Precondition 1: The user is logged into the system
        PerformLogin(eventPage);

        // Precondition 2: The user navigates to the "Create event" page via the UI
        eventPage.NavigateToCreateEventPageFromProfile();

        // --- Act & Assert ---

        // Step 1: Click on the "Title" field and enter the event name
        eventPage.TitleField.Clear();
        eventPage.TitleField.SendKeys(EventTitle);
        Assert.That(eventPage.TitleField.GetAttribute("value"), Is.EqualTo(EventTitle), 
            "Step 1 Failed: The event title text was not successfully entered into the field.");

        // Step 2: Check the "Duration" dropdown
        string actualDuration = eventPage.DurationDropdown.GetSelectedOptionText();
        Assert.That(actualDuration, Is.EqualTo(ExpectedDefaultDuration), 
            "Step 2 Failed: The 'Duration' field is not set to default '1 day'.");

        // Step 3: Select "Initiative type" (Environmental)
        eventPage.InitiativeTypeButton(InitiativeTypeValue).Click();
        Assert.That(eventPage.IsInitiativeTypeActive(InitiativeTypeValue), Is.True, 
            $"Step 3 Failed: The '{InitiativeTypeValue}' button did not become active.");

        // Step 4: Select an option from the "Event type" dropdown
        eventPage.EventTypeDropdown.Click();
        eventPage.EventTypeDropdown.ClickDropDownOptionByPartialName(EventTypeValue);
        Assert.That(eventPage.EventTypeDropdown.GetSelectedOptionText(), Is.EqualTo(EventTypeValue), 
            $"Step 4 Failed: '{EventTypeValue}' is not displayed as the selected option.");

        // Step 5: Select an option from the mandatory dropdown next to Event type (Invite)
        eventPage.InviteDropdown.Click();
        eventPage.InviteDropdown.ClickDropDownOptionByPartialName(InviteValue);
        Assert.That(eventPage.InviteDropdown.GetSelectedOptionText(), Is.EqualTo(InviteValue), 
            $"Step 5 Failed: '{InviteValue}' is not displayed as the selected option in the Invite dropdown.");

        // Step 6: Enter valid text into the "Events Description" field
        eventPage.DescriptionEditor.Clear();
        eventPage.DescriptionEditor.SendKeys(DescriptionValue);
        Assert.That(eventPage.DescriptionEditor.Text, Does.Contain(DescriptionValue), 
            "Step 6 Failed: The description text is not displayed in the editor.");

        // Step 7: Select a date in the "Choose a date" field
        eventPage.DatePicker.EnterDate(dynamicDateValue);
        Assert.That(eventPage.DatePicker.GetSelectedDate(), Is.EqualTo(dynamicDateValue), 
            "Step 7 Failed: The selected date is not displayed correctly in the field.");

        // Step 8: Enter the start time in the "Start Time" field
        eventPage.StartTimeInput.Clear();
        eventPage.StartTimeInput.SendKeys(StartTimeValue);
        Assert.That(eventPage.StartTimeInput.GetAttribute("value"), Is.EqualTo(StartTimeValue), 
            "Step 8 Failed: The start time was not set correctly.");

        // Step 9: Enter the end time in the "End Time" field
        eventPage.EndTimeInput.Clear();
        eventPage.EndTimeInput.SendKeys(EndTimeValue);
        Assert.That(eventPage.EndTimeInput.GetAttribute("value"), Is.EqualTo(EndTimeValue), 
            "Step 9 Failed: The end time was not set correctly.");

        // Step 10: Check the event format checkbox (Online)
        eventPage.OnlineLocationCheckbox.Check();
        Assert.That(eventPage.OnlineLocationCheckbox.IsChecked(), Is.True, 
            "Step 10 Failed: The 'Online' checkbox is not ticked.");

        // Step 11: Click on the "Online link" field and enter the link
        eventPage.OnlineLinkField.Clear();
        eventPage.OnlineLinkField.SendKeys(OnlineLinkValue);
        Assert.That(eventPage.OnlineLinkField.GetAttribute("value"), Is.EqualTo(OnlineLinkValue), 
            "Step 11 Failed: The online link was not successfully entered into the field.");

        // Step 12: Check the "Apply to all days of the event" checkbox
        eventPage.ApplyToAllDaysCheckbox.Check();
        Assert.That(eventPage.ApplyToAllDaysCheckbox.IsChecked(), Is.True, 
            "Step 12 Failed: The 'Apply to all days' checkbox is not ticked.");

        // Step 13: Scroll down to the "Picture" section
        actions.ScrollToElement(eventPage.AddPictureButton).Perform();

        // Step 14: Select an image from "Use Greencity pictures" (first picture)
        eventPage.GreencityPictureItem(1).Click();
        Assert.Multiple(() =>
        {
            Assert.That(eventPage.UploadedImagePreview.Displayed, Is.True, 
                "Step 14 Failed: The image was not added to the upload block.");
            Assert.That(eventPage.MainBadge.Displayed, Is.True, 
                "Step 14 Failed: The image did not receive the 'Main' badge.");
            Assert.That(eventPage.ClosePictureIcon.Displayed, Is.True, 
                "Step 14 Failed: The 'X' (close) button is not displayed on the image.");
            Assert.That(eventPage.PictureCounter.Text, Does.Contain(ExpectedCounterValue), 
                $"Step 14 Failed: The counter did not update to '{ExpectedCounterValue}'.");
        });

        // Step 15: Hover over the "Publish" button
        actions.MoveToElement(eventPage.PublishButton).Perform();
        Assert.That(eventPage.PublishButton.Enabled, Is.True, 
            "Step 15 Failed: The 'Publish' button did not become active.");

        // Step 16: Click the "Publish" button
        eventPage.PublishButton.Click();

        // Wait for the redirection to execute fully using encapsulated wait
        bool isRedirected = eventPage.WaitForUrlToContain(ExpectedEventsUrlSubstring);
        
        Assert.Multiple(() =>
        {
            Assert.That(isRedirected, Is.True, 
                "Step 16 Failed: The user was not redirected to the Events page within the given timeout period.");
            Assert.That(eventPage.SuccessSnackBar.Displayed, Is.True, 
                "Step 16 Failed: The success message toast is not displayed.");
            Assert.That(Driver!.Url, Does.Contain(ExpectedEventsUrlSubstring), 
                "Step 16 Failed: The URL does not contain the expected events substring.");
        });
    }
}

// test windows push
