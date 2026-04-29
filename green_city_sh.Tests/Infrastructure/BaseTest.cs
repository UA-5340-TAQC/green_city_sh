using OpenQA.Selenium;
using green_city_sh.Tests.Drivers;
using NUnit.Allure.Core;

namespace green_city_sh.Tests.Infrastructure;

[TestFixture]
[AllureNUnit]
public abstract class BaseTest
{
    protected IWebDriver? Driver { get; private set; }
    protected string BaseUrl { get; set; } = Configuration.BaseUrl;

    [SetUp]
    public void Setup()
    {
        Driver = DriverFactory.CreateDriver(BrowserType.Chrome);
        OnSetup();
    }

    [TearDown]
    public void TearDown()
    {
        OnTearDown();

        if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
        {
            TakeScreenshot(TestContext.CurrentContext.Test.Name);
        }

        Driver?.Quit();
        Driver?.Dispose();
    }

    protected virtual void OnSetup()
    {
        // Override this method in derived test classes for custom setup
    }

    protected virtual void OnTearDown()
    {
        // Override this method in derived test classes for custom teardown
    }

    protected void NavigateToBaseUrl()
    {
        Driver?.Navigate().GoToUrl(BaseUrl);
    }

    private void TakeScreenshot(string testName)
    {
        try
        {
            var screenshot = ((ITakesScreenshot)Driver!).GetScreenshot();
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var fileName = $"{testName}_{timestamp}.png";
            var screenshotPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Screenshots", fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(screenshotPath)!);
            screenshot.SaveAsFile(screenshotPath);

            TestContext.WriteLine($"Screenshot saved: {screenshotPath}");
        }
        catch (Exception ex)
        {
            TestContext.WriteLine($"Failed to take screenshot: {ex.Message}");
        }
    }
}
