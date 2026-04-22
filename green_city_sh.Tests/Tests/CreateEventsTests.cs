using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;
using green_city_sh.Tests.Components;

namespace green_city_sh.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    public class CreateEventsTests : BaseTest
    {
        private CreateUpdateEventPage? createEventPage;

        [SetUp]
        public void SetUp()
        {
            var wait = new WebDriverWait(Driver!, TimeSpan.FromSeconds(Configuration.DefaultTimeout));

            Driver!.Navigate().GoToUrl("https://www.greencity.cx.ua/#/greenCity");

            var eventsTab = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a[href='#/greenCity/events']")));
            eventsTab.Click();

            var signInHeaderBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[contains(text(), 'Sign in')]")));
            signInHeaderBtn.Click();

            var signInModal = SignInModalComponent.WaitAndCreate(Driver!);
            signInModal.Login("greencitytest69@hotmail.com", "asweQA5346!)");

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    var eventsTabAfterLogin = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a[href='#/greenCity/events']")));
                    eventsTabAfterLogin.Click();
                    break;
                }
                catch (StaleElementReferenceException) { Thread.Sleep(500); }
            }

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    var createEventBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(., 'Create event') or contains(@class, 'create-btn')]")));
                    createEventBtn.Click();
                    break;
                }
                catch (StaleElementReferenceException) { Thread.Sleep(500); }
            }

            createEventPage = new CreateUpdateEventPage(Driver!);
        }

        [Test]
        [Category("Smoke")]
        public void VerifyUserCanCreateNewEvent()
        {
            createEventPage!.SetTitle("Green City Cleanup Festival 2026");
            createEventPage.SelectInitiativeType("Social");
            createEventPage.SelectEventType("Open");
            createEventPage.SetDescription("Join us to clean the park and make our city greener!");

            string tomorrow = DateTime.Now.AddDays(1).ToString("MM/dd/yyyy");
            createEventPage.SetDate(tomorrow);
            createEventPage.SetTime("21:00", "22:00");

            createEventPage.SelectPlaceLocation();
            createEventPage.EnterAddress("Mitskevycha Square, 14, Ivano-Frankivs'k");
            createEventPage.SelectAddressFromDropdown();

            // Error fix!!!
            Driver!.FindElement(By.CssSelector("input[formcontrolname='place']")).Click();
            Thread.Sleep(1000);

            createEventPage.ClickPreview();
            createEventPage.ClickBackToEditing();
            createEventPage.ClickPublish();

            var wait = new WebDriverWait(Driver!, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/events"));

            Assert.IsTrue(Driver!.Url.Contains("/events"), "Redirection failed");
        }

        [TearDown]
        public void LocalTearDown()
        {
            // Postconditions: Delete created event
        }
    }
}