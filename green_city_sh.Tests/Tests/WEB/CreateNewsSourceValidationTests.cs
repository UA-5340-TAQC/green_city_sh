using Allure.Net.Commons;
using Allure.Net.Commons.Attributes;
using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace green_city_sh.Tests.Tests.WEB;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
[AllureOwner("Nikita Muntianov")]
[AllureSubSuite("Create News")]
public class CreateNewsSourceValidationTests : BaseUITest
{
    // --- Test Data ---
    private const string TestTitleBase = "TC-043 Unique News Title";
    private const string TestContent = "Hello! It's my news for today!";
    private const string InvalidUrlNoProtocol = "google.com";
    private const string InvalidUrlFtp = "ftp://example.com";
    private const string SourceValidationMessage = "Link must start with http(s)://";
    private const string DefaultTag = "News";

    private bool isNewsCreated = false;
    private CreateNewsPage? createNewsPage;
    private NewsPage? newsPage;

    private const string UbsUrlSubstring = "/ubs";
    private const string NewsUrlSubstring = "/news";

    protected override void OnSetup()
    {
        NavigateToBaseUrl();

        var wait = new WebDriverWait(Driver!, TimeSpan.FromSeconds(Configuration.DefaultTimeout));
        var signInBtn = wait.Until(ExpectedConditions.ElementToBeClickable(
            By.XPath("//*[contains(@class, 'sign-in') or contains(text(), 'Sign in')]")));
        signInBtn.Click();

        var signInModal = SignInModalComponent.WaitAndCreate(Driver!);
        signInModal.Login(Configuration.TestEmail, Configuration.TestPassword);
        wait.Until(ExpectedConditions.InvisibilityOfElementLocated(SignInModalComponent.RootLocator));
    }

    [Test]
    [Category("News")]
    [AllureDescription("Validate correct URL format (with http/https)")]
    [AllureSuite("GreenCity")]
    [AllureSubSuite("News")]
    [AllureTag("UI", "Functional", "Validation")]
    [AllureIssue("TC-043")]
    [AllureSeverity(SeverityLevel.critical)]
    public void VerifySourceFieldValidation()
    {
        // --- Arrange ---
        createNewsPage = new CreateNewsPage(Driver!);
        createNewsPage.OpenCreateNewsPage();

        string uniqueTitle = $"{TestTitleBase} {Guid.NewGuid()}";

        createNewsPage.EnterTitle(uniqueTitle);
        createNewsPage.SelectTags(DefaultTag);
        createNewsPage.EnterContent(TestContent);

        // --- Act & Assert 1 (No Protocol) ---
        createNewsPage.EnterSource(InvalidUrlNoProtocol);

        Assert.Multiple(() =>
        {
            Assert.That(
                () => createNewsPage.IsSourceFieldInvalid(),
                Is.True.After(1000, 100),
                "Source field should be invalid when URL lacks protocol.");
            Assert.That(
                createNewsPage.GetSourceFieldInfoText(),
                Does.Contain(SourceValidationMessage),
                "Validation message should indicate the correct protocol is required.");
            Assert.That(
                () => createNewsPage.IsPublishButtonEnabled(),
                Is.False.After(1000, 100),
                "Publish button should be disabled for invalid URL.");
        });

        // --- Act & Assert 2 (FTP Protocol) ---
        createNewsPage.ClearAndBlurSourceField();
        createNewsPage.EnterSource(InvalidUrlFtp);

        Assert.Multiple(() =>
        {
            Assert.That(
                () => createNewsPage.IsSourceFieldInvalid(),
                Is.True.After(1000, 100),
                "Source field should be invalid for FTP protocol.");
            Assert.That(
                () => createNewsPage.IsPublishButtonEnabled(),
                Is.False.After(1000, 100),
                "Publish button should be disabled for FTP protocol.");
        });

        // --- Act & Assert 3 (Empty Optional Field) ---
        createNewsPage.ClearAndBlurSourceField();

        Assert.Multiple(() =>
        {
            Assert.That(
                () => createNewsPage.IsSourceFieldInvalid(),
                Is.False.After(1000, 100),
                "Source field should not be invalid when empty (it is optional).");
            Assert.That(
                () => createNewsPage.IsPublishButtonEnabled(),
                Is.True.After(1000, 100),
                "Publish button should be enabled when optional field is empty and other fields are valid.");
        });

        // --- Act & Assert 4 (Publish & Verify) ---
        createNewsPage.ClickPublish();
        createNewsPage.WaitForUrlToContain(UbsUrlSubstring);
        isNewsCreated = true;

        Driver!.Navigate().GoToUrl(BaseUrl + NewsUrlSubstring);
        newsPage = new NewsPage(Driver!);
        newsPage.List.WaitForCardsToLoad();
        newsPage.List.OpenNewsByIndex(0);

        var newsDetailsPage = new NewsDetailsPage(Driver!);

        Assert.That(newsDetailsPage.IsSourceLinkDisplayed(), Is.False,
            "Source link should not be displayed when the source field was left empty.");
    }

    [TearDown]
    [AllureStep("Tear Down TC-043 Environment")]
    public override void TearDown()
    {
        if (isNewsCreated)
        {
            try
            {
                Driver!.Navigate().GoToUrl(BaseUrl + NewsUrlSubstring);
                var cleanupNewsPage = new NewsPage(Driver!);
                cleanupNewsPage.List.WaitForCardsToLoad();
                cleanupNewsPage.List.OpenNewsByIndex(0);

                var newsDetailsPage = new NewsDetailsPage(Driver!);
                newsDetailsPage.ClickDeleteNewsButton();
                newsDetailsPage.ConfirmDeletion();
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"Failed to delete the created news during teardown: {ex.Message}");
            }
        }

        base.TearDown();
    }
}
