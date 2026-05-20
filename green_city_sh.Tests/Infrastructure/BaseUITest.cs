using Allure.Net.Commons;
using Allure.Net.Commons.Attributes;
using green_city_sh.Tests.Drivers;
using OpenQA.Selenium;

namespace green_city_sh.Tests.Infrastructure
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    [Allure.NUnit.AllureNUnit]
    [AllureSuite("UI Tests")]
    public abstract class BaseUITest
    {
        protected IWebDriver? Driver { get; private set; }
        protected string BaseUrl { get; set; } = Configuration.BaseUrl;

        [SetUp]
        [AllureStep("Setup Test Environment")]
        public void Setup()
        {
            Driver = DriverFactory.CreateDriver(BrowserType.Chrome);
            OnSetup();
        }

        [TearDown]
        [AllureStep("Tear Down Test Environment")]
        public virtual void TearDown()
        {
            try
            {
                if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
                {
                    TakeScreenshot(TestContext.CurrentContext.Test.Name);
                }
                OnTearDown();
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"Error during TearDown: {ex.Message}");
            }
            finally
            {
                Driver?.Quit();
                Driver?.Dispose();
            }
        }

        protected virtual void OnSetup() { }
        protected virtual void OnTearDown() { }

        [AllureStep("Navigate to Base URL")]
        protected void NavigateToBaseUrl() => Driver!.Navigate().GoToUrl(BaseUrl);

        protected void TakeScreenshot(string testName)
        {
            try
            {
                if (Driver == null) return;

                var screenshot = ((ITakesScreenshot)Driver).GetScreenshot().AsByteArray;

                AllureApi.AddAttachment(
                    name: $"Screenshot_{testName}",
                    type: "image/png",
                    content: screenshot,
                    fileExtension: ".png"
                );
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"Failed to take screenshot: {ex.Message}");
            }
        }
    }
}
