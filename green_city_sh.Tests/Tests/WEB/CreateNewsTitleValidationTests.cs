using Allure.Net.Commons;
using Allure.Net.Commons.Attributes;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;
using green_city_sh.Tests.Components;

namespace green_city_sh.Tests.Tests.WEB;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
[AllureOwner("Nikita Muntianov")]
[AllureFeature("Create News")]
public class CreateNewsTitleValidationTests : BaseUITest
{
    private bool isNewsCreated = false;
    private CreateNewsPage? createNewsPage;
    private NewsPage? newsPage;

    private void PerformLogin()
    {
        NavigateToBaseUrl();
        var wait = new WebDriverWait(Driver!, TimeSpan.FromSeconds(Configuration.DefaultTimeout));
        var signInBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(@class, 'sign-in') or contains(text(), 'Sign in')]")));
        signInBtn.Click();

        var signInModal = SignInModalComponent.WaitAndCreate(Driver!);
        signInModal.Login(Configuration.TestEmail, Configuration.TestPassword);
        wait.Until(ExpectedConditions.InvisibilityOfElementLocated(SignInModalComponent.RootLocator));
    }

    [Test]
    [AllureDescription("Validate Title field (required, max length 170, validation behavior)")]
    [AllureSuite("GreenCity")]
    [AllureSubSuite("News")]
    [AllureTag("UI", "Functional")]
    [AllureIssue("TC-042")]
    [Category("News")]
    [AllureSeverity(SeverityLevel.critical)]
    public void VerifyTitleFieldValidationAndCreation()
    {
        // --- Arrange ---
        PerformLogin();

        createNewsPage = new CreateNewsPage(Driver!);
        createNewsPage.OpenCreateNewsPage();

        string validContent = new string('B', 25);
        string title170 = new string('A', 170);
        string title171 = new string('A', 171);

        // --- Act (Checkpoint 1: Form Validation & Empty State) ---
        string initialTitle = createNewsPage.NewsForm.GetTitleValue();
        createNewsPage.EnterContent(validContent);
        createNewsPage.SelectTags("News");
        bool isPublishEnabledInitially = createNewsPage.IsPublishButtonEnabled();

        createNewsPage.FocusAndBlurTitleField();
        bool isTitleTouchedAndInvalidAfterBlur = createNewsPage.IsTitleFieldTouchedAndInvalid();

        createNewsPage.EnterTitle("Temp");
        createNewsPage.ClearAndBlurTitleField();
        bool isTitleInvalidAfterDelete = createNewsPage.IsTitleFieldTouchedAndInvalid();

        // --- Assert (Checkpoint 1) ---
        Assert.Multiple(() =>
        {
            Assert.That(initialTitle, Is.Null.Or.Empty, "Step 1: Title field should be empty initially.");
            Assert.That(isPublishEnabledInitially, Is.False, "Step 4: Publish button should be disabled when title is empty.");
            Assert.That(isTitleTouchedAndInvalidAfterBlur, Is.True, "Step 5: Red border should be displayed after focusing and blurring without entering text.");
            Assert.That(isTitleInvalidAfterDelete, Is.True, "Step 6: Red border should be displayed after entering and deleting title.");
        });

        // --- Act (Checkpoint 2: Max Length Enforcement) ---
        createNewsPage.EnterTitle(title171);
        bool isTitleInvalidFor171 = createNewsPage.IsTitleFieldInvalid();
        bool isPublishEnabledFor171 = createNewsPage.IsPublishButtonEnabled();

        createNewsPage.EnterTitle(title170);
        bool isTitleInvalidFor170 = createNewsPage.IsTitleFieldInvalid();
        bool isPublishEnabledFor170 = createNewsPage.IsPublishButtonEnabled();

        // --- Assert (Checkpoint 2) ---
        Assert.Multiple(() =>
        {
            Assert.That(isTitleInvalidFor171, Is.True, "Step 8: Title with 171 characters should trigger error or limit enforcement.");
            Assert.That(isPublishEnabledFor171, Is.False, "Publish button should be disabled for 171 characters.");
            Assert.That(isTitleInvalidFor170, Is.False, "Step 9: Title with 170 characters should be valid.");
            Assert.That(isPublishEnabledFor170, Is.True, "Step 10: Publish button should be enabled for valid 170 characters title.");
        });

        // --- Act (Checkpoint 3: Creation & Redirection) ---
        createNewsPage.ClickPublish();

        createNewsPage.WaitForUrlToContain("/ubs");

        isNewsCreated = true;

        Driver!.Navigate().GoToUrl(BaseUrl + "/#/greenCity/news");
        newsPage = new NewsPage(Driver!);
        newsPage.List.WaitForCardsToLoad();
        newsPage.List.OpenNewsByIndex(0);

        var newsDetailsPage = new NewsDetailsPage(Driver!);
        string displayedTitle = newsDetailsPage.GetNewsTitleText();

        Driver!.Navigate().Refresh();
        string titleAfterRefresh = newsDetailsPage.GetNewsTitleText();

        Driver!.Navigate().Back();
        newsPage.List.WaitForCardsToLoad();
        string returnUrl = Driver!.Url;

        // --- Assert (Checkpoint 3) ---
        Assert.Multiple(() =>
        {
            Assert.That(displayedTitle, Is.EqualTo(title170), "Step 13: Correct title should be displayed on the news details page.");
            Assert.That(titleAfterRefresh, Is.EqualTo(title170), "Step 14: Data should persists after page refresh.");
            Assert.That(returnUrl, Does.Contain("/news"), "Step 15: Should return to the news list page after navigating back.");
        });
    }

    [TearDown]
    public void TearDownCreatedNews()
    {
        if (isNewsCreated)
        {
            try
            {
                Driver!.Navigate().GoToUrl(BaseUrl + "/#/greenCity/news");
                var newsListPage = new NewsPage(Driver!);
                newsListPage.List.WaitForCardsToLoad();
                newsListPage.List.OpenNewsByIndex(0);

                var newsDetailsPage = new NewsDetailsPage(Driver!);
                newsDetailsPage.ClickDeleteNewsButton();
                newsDetailsPage.ConfirmDeletion();
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"Failed to delete the created news during teardown: {ex.Message}");
            }
        }
    }
}
