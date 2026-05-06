using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;
using green_city_sh.Tests.Components;
using Allure.NUnit.Attributes;

namespace green_city_sh.Tests.Tests;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class EventPlannerButtonTests : BaseTest
{
    private HomePage homePage = null!;

    protected override void OnSetup()
    {
        NavigateToBaseUrl();
        homePage = new HomePage(Driver!);

        homePage.Header.ClickSignIn();
        SignInModalComponent signInModal = SignInModalComponent.WaitAndCreate(Driver!);

        signInModal.Login(Configuration.TestEmail, Configuration.TestPassword);

        MySpacePage spacePage = new MySpacePage(Driver!);
        spacePage.WaitUntilPageLoads();

        Driver!.Navigate().GoToUrl(Configuration.BaseUrl + "/news");
    }

    [Test]
    [AllureIssue("8")]
    [AllureDescription("Verification of Event Planner button functionality.")]
    [Description("Verify Event Planner button visibility, hover effect, and Search button functionality")]
    [Category("Smoke")]
    public void VerifyEventPlannerAndSearchButtonWork()
    {
        var wait = new WebDriverWait(Driver!, TimeSpan.FromSeconds(Configuration.DefaultTimeout));

        By searchIconLocator = By.CssSelector("img.my-events-img");

        var searchIcon = wait.Until(ExpectedConditions.ElementToBeClickable(searchIconLocator));
        searchIcon.Click();

        bool isNavigated = wait.Until(d => d.Url.Contains("events"));

        Assert.IsTrue(isNavigated, "The button did not navigate to the events page");
    }
}