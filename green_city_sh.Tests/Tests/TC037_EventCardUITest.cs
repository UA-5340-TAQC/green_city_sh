using NUnit.Framework.Interfaces;
using System.Text.RegularExpressions;
using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;



namespace green_city_sh.Tests.Tests;

[TestFixture]
public class TC037_EventCardUITest : BaseTest
{
    private string TestEmail = null!;
    private string TestPassword = null!;

    
    private static readonly HashSet<string> AllowedCategories =
        new(StringComparer.OrdinalIgnoreCase) { "Environmental", "Social", "Economic" };
    
    private static readonly HashSet<string> AllowedStatuses =
        new() {"Open", "Closed"};

    private static readonly Regex DateFormatRegex =
        new(@"^[A-Z][a-z]{2}\s\d{1,2},\s\d{4}$", RegexOptions.Compiled);

    private static readonly Regex TimeFormatRegex =
        new(@"^\d{1,2}:\d{2}\s(AM|PM)$", RegexOptions.Compiled);
    
    private EventCardComponent? firstCard;

    protected override void OnSetup()
    {
        TestEmail = Environment.GetEnvironmentVariable("TEST_EMAIL")
                                          ?? throw new InvalidOperationException("TEST_EMAIL is not configured.");
        TestPassword = Environment.GetEnvironmentVariable("TEST_PASSWORD") 
                                             ?? throw new InvalidOperationException("TEST_PASSWORD is not configured.");
        NavigateToBaseUrl();
        
        var header = new HeaderComponent(Driver!, HeaderComponent.RootLocator);
        header.ChangeLanguage("En");
        header.ClickSignIn();
        
        SignInModalComponent
            .WaitAndCreate(Driver!)
            .Login(TestEmail, TestPassword);
        
        header.WaitForUserLoggedIn();
        
        var eventsPage = new EventsPage(Driver!);
        eventsPage.OpenEventsPage();
        eventsPage.WaitForFirstCardVisible();

        firstCard = eventsPage.GetFirstEventCard();
    }
    
    [Test]
    [Category("Smoke")]
    public void TC037_Step1_FirstEventCard_IsVisible()
    {
        Assert.That(firstCard!.GetImage().Displayed,
            "Step 1 FAILED: First event card is not visible on the Events page.");
    }

    [Test]
    [Category("Smoke")]
    public void TC037_Step2_EventImage_IsPresentAndLoaded()
    {
        Assert.That(firstCard!.GetImage().Displayed,
            "Step 2 FAILED: Event image element is not displayed.");
        
        Assert.That(firstCard!.GetImageSrc(), Is.Not.Empty, 
            "Step 2 FAILED: Event image 'src' attribute is empty or missing.");
        
        Assert.That(firstCard!.IsImageLoaded(),
            $"Step 2 FAILED: Event image is not fully loaded.");
    }

    [Test]
    [Category("Smoke")]
    public void TC037_Step3_CategoryTag_IsValidValue()
    {
        
        var tagText = firstCard!.GetCategoryText();
        Assert.That(tagText, Is.Not.Empty,
            "Step 3 FAILED: Category tag text is empty.");
        Assert.That(AllowedCategories.Contains(tagText),
                $"Step 3 FAILED: Category tag value '{tagText}' is not one of " +
                $"[{string.Join(", ", AllowedCategories)}].");
    }

    [Test]
    [Category("Smoke")]
    public void TC037_Step4_DateTimeFormat_IsValid()
    {
        var dateText = firstCard!.GetDateText();
        var timeText = firstCard!.GetTimeText();
        
        Assert.That(dateText, Is.Not.Empty,
            "Step 4 FAILED: Date label text is empty.");
        Assert.That(DateFormatRegex.IsMatch(dateText),
            $"Step 4 FAILED: Date/time text '{dateText}' does not match " +
            $"expected format 'MMM d, yyyy'.");
        
        Assert.That(timeText, Is.Not.Empty,
            "Step 4 FAILED: Time label text is empty.");
        Assert.That(TimeFormatRegex.IsMatch(timeText),
            $"Step 4 FAILED: Time text '{timeText}' does not match " +
            $"expected format 'h:mm AM/PM'.");
    }

    [Test]
    [Category("Smoke")]
    public void TC037_Step5_EventStatus_IsOpenOrClosed()
    {
        var statusText = firstCard!.GetStatusTest();
        
        Assert.That(statusText, Is.Not.Empty,
            "Step 5 FAILED: Status text is empty.");
        Assert.That(AllowedStatuses.Contains(statusText),
            $"Step 5 FAILED: Status '{statusText}' is not 'Open' or  'Closed'.");
    }

    [Test]
    [Category("Smoke")]
    public void TC037_Step6_ActionButtons_ArePresentAndEnabled()
    {
        var buttonTexts = firstCard!.GetActionButtonTexts();

        Assert.That(buttonTexts.Count, Is.GreaterThanOrEqualTo(1),
            $"Step 6 FAILED: Expected at least 1 action buttons, found {buttonTexts.Count}.");
        
        Assert.That(buttonTexts, Has.Member("More"),
            $"Step 6 FAILED: 'More' button not found. Buttons present: [{string.Join(", ", buttonTexts)}]");
        
        Assert.That(firstCard.AreAllActionButtonsEnabled(),
            "Step 6 FAILED: One or more action buttons are disabled.");

    }
}
