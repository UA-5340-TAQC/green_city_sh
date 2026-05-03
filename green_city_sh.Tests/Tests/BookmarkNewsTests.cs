using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Threading;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;
using green_city_sh.Tests.Components;
using Allure.NUnit.Attributes;

namespace green_city_sh.Tests.Tests;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class NewsBookmarkTests : BaseTest
{
    private NewsPage _newsPage = null!;
    private string _targetNewsTitle = string.Empty;

    protected override void OnSetup()
    {
        NavigateToBaseUrl();
        var homePage = new HomePage(Driver!);
        
        homePage.Header.ClickSignIn();
        var signInModal = SignInModalComponent.WaitAndCreate(Driver!);
        signInModal.Login(Configuration.TestEmail, Configuration.TestPassword);
        
        var spacePage = new MySpacePage(Driver!);
        spacePage.WaitUntilPageLoads();

        Driver!.Navigate().GoToUrl(Configuration.BaseUrl + "/news");
        _newsPage = new NewsPage(Driver!);
        
        _newsPage.List.WaitForCardsToLoad();
    }

    [Test]
    [AllureIssue("30")]
    [AllureDescription("Check that user can bookmark a news item and it appears in the saved news section with correct counter update.")]
    [Category("Functional")]
    public void VerifyUserCanBookmarkNewsItem()
    {
        var allCards = _newsPage.List.GetAllNewsCardsAsComponents();
        var targetCard = allCards.FirstOrDefault(); 
        
        Assert.IsNotNull(targetCard, "No news cards found on the page to bookmark.");
        _targetNewsTitle = targetCard!.GetTitle();

        targetCard.ClickBookmark();

        _newsPage.TopBar.OpenSavedNews(); 

        _newsPage.TopBar.WaitForCounterToAppear();

        var savedCards = _newsPage.List.GetAllNewsCardsAsComponents();
        
        bool isPresent = savedCards.Any(c => 
            c.GetTitle().Trim().Equals(_targetNewsTitle.Trim(), StringComparison.OrdinalIgnoreCase));
        
        Assert.IsTrue(isPresent, 
            $"Error: News '{_targetNewsTitle}' not found in the saved news list. " +
            $"Found a total of {savedCards.Count} saved news items.");

        int itemsCount = _newsPage.TopBar.GetItemsFoundCount();
        Assert.IsTrue(itemsCount > 0, $"Error: Counter shows {itemsCount}, but it should be > 0.");
    }

    [TearDown]
    public void LocalTearDown()
    {
        try 
        {
            Driver!.Navigate().GoToUrl(Configuration.BaseUrl + "/news");
            _newsPage = new NewsPage(Driver!);
            _newsPage.List.WaitForCardsToLoad();
            
            var cards = _newsPage.List.GetAllNewsCardsAsComponents();
            var targetCard = cards.FirstOrDefault(c => c.GetTitle().Contains(_targetNewsTitle));
            
            if (targetCard != null && targetCard.IsBookmarked())
            {
                targetCard.ClickBookmark();
            }
        }
        catch 
        {
        }
    }
}