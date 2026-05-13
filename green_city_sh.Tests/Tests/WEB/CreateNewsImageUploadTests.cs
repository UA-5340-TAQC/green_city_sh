using Allure.Net.Commons;
using Allure.Net.Commons.Attributes;
using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace green_city_sh.Tests.Tests.WEB;

[AllureOwner("Nikita Muntianov")]
[AllureSubSuite("Create News")]
public class CreateNewsImageUploadTests : BaseUITest
{
    private const string InvalidFileName = "invalidFile.pdf";
    private const string ValidFileName = "validImage.jpg";
    private const string FormatValidationMessage = "Upload only PNG or JPG";
    private const string TestTitleBase = "TC-044 Valid Image Upload";
    private const string TestContent = "Hello! It's my news for today!";
    private const string DefaultTag = "News";

    private const string UbsUrlSubstring = "/ubs";
    private const string NewsUrlSubstring = "/news";

    private bool isNewsCreated = false;
    private CreateNewsPage? createNewsPage;
    private NewsPage? newsPage;

    protected override void OnSetup()
    {
        NavigateToBaseUrl();

        var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(Driver!, TimeSpan.FromSeconds(Configuration.DefaultTimeout));
        var signInBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(@class, 'sign-in') or contains(text(), 'Sign in')]")));
        signInBtn.Click();

        var signInModal = SignInModalComponent.WaitAndCreate(Driver!);
        signInModal.Login(Configuration.TestEmail, Configuration.TestPassword);
        wait.Until(ExpectedConditions.InvisibilityOfElementLocated(SignInModalComponent.RootLocator));
    }

    [Test]
    [AllureDescription("Validate image upload format (only PNG/JPG allowed)")]
    [AllureFeature("News")]
    [AllureTag("UI", "Functional", "Validation")]
    [AllureIssue("TC-044")]
    [AllureSeverity(SeverityLevel.critical)]
    public void VerifyImageUploadFormatValidation()
    {
        // --- Arrange ---
        createNewsPage = new CreateNewsPage(Driver!);
        createNewsPage.OpenCreateNewsPage();

        // --- Act & Assert 1 (Invalid Format) ---
        createNewsPage.UploadImage(InvalidFileName);

        Assert.Multiple(() =>
        {
            Assert.That(createNewsPage.GetImageUploadWarningMessage(), Does.Contain(FormatValidationMessage),
                "Warning message should indicate that only PNG or JPG are allowed.");
            Assert.That(() => createNewsPage.IsPublishButtonEnabled(), Is.False.After(1000, 100),
                "Publish button should remain disabled when an invalid image format is uploaded.");
        });

        // --- Act & Assert 2 (Valid Format) ---
        createNewsPage.UploadImage(ValidFileName);

        Assert.That(() => createNewsPage.IsImagePreviewDisplayed(), Is.True.After(1000, 100),
            "Image preview should be displayed after uploading a valid image format.");

        createNewsPage.SubmitImageCrop();

        // --- Act & Assert 3 (Publish & Verify) ---
        string uniqueTitle = $"{TestTitleBase} {Guid.NewGuid()}";
        createNewsPage.EnterTitle(uniqueTitle);
        createNewsPage.SelectTags(DefaultTag);
        createNewsPage.EnterContent(TestContent);

        createNewsPage.ClickPublish();
        createNewsPage.WaitForUrlToContain(UbsUrlSubstring);
        isNewsCreated = true;

        Driver!.Navigate().GoToUrl(BaseUrl + NewsUrlSubstring);
        newsPage = new NewsPage(Driver!);
        newsPage.List.WaitForCardsToLoad();
        newsPage.List.OpenNewsByIndex(0);

        var newsDetailsPage = new NewsDetailsPage(Driver!);

        Assert.That(newsDetailsPage.IsImageDisplayed(), Is.True,
            "The uploaded image should be visible on the News Details page.");
    }
    [TearDown]
    [AllureStep("Tear Down TC-044 Environment")]
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
