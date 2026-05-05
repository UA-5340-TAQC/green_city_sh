using OpenQA.Selenium;
using green_city_sh.Tests.Drivers;
using NUnit.Framework;
using Allure.Net.Commons;
using System.IO;

namespace green_city_sh.Tests.Infrastructure
{
    [TestFixture]
    [Allure.NUnit.AllureNUnit]
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

        protected void NavigateToBaseUrl() => Driver?.Navigate().GoToUrl(BaseUrl);

        protected void TakeScreenshot(string testName)
        {
            try
            {
                if (Driver == null) return;

                var screenshot = ((ITakesScreenshot)Driver).GetScreenshot().AsByteArray;

                // Найбільш універсальний спосіб для Allure C# адаптера:
                AllureApi.AddAttachment(
                    name: $"Screenshot_{testName}",
                    type: "image/png",
                    content: screenshot,
                    fileExtension: ".png"
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не вдалося прикріпити скріншот до Allure: {ex.Message}");
            }
        }
    }
}