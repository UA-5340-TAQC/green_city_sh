using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Components;

namespace green_city_sh.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    public class LoginTests : BaseTest
    {
        [Test]
        [Category("Smoke")]
        public void VerifySuccessfulLoginWithValidCredentials()
        {
            Driver!.Navigate().GoToUrl("https://www.greencity.cx.ua/#/greenCity");
            var wait = new WebDriverWait(Driver!, TimeSpan.FromSeconds(10));
            var header = new HeaderComponent(Driver!, By.TagName("header"));

            header.ClickSignIn();
            var signInModal = SignInModalComponent.WaitAndCreate(Driver!);

            signInModal.Login("greencitytest69@hotmail.com", "asweQA5346!)");
            var userProfileBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#header_user-wrp")));

            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver!;
            string? accessToken = (string?)js.ExecuteScript("return window.localStorage.getItem('accessToken');");
            Assert.IsFalse(string.IsNullOrEmpty(accessToken), "Session token was not created in LocalStorage.");

            wait.Until(d => d.Url.Contains("/profile"));
            Assert.IsTrue(Driver!.Url.Contains("/profile"), "User is not on the profile page.");

            userProfileBtn.Click();
            var signOutOption = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[aria-label='sign-out']")));
            Assert.IsTrue(signOutOption.Displayed, "'Sign out' button is not visible.");
        }
    }
}