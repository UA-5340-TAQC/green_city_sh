using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;
using OpenQA.Selenium;
using Allure.NUnit;
using Allure.NUnit.Attributes;

namespace green_city_sh.Tests.Tests;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class NewsPageTests : BaseTest
{
    private NewsPage? newsPage;
    private HeaderComponent? header;

    protected override void OnSetup()
    {
        Driver!.Manage().Window.Maximize();
        NavigateToBaseUrl();
        header = new HeaderComponent(Driver, Driver!.FindElement(By.CssSelector("header")));

        header.ChangeLanguage("En");
        header.ClickSignIn();
        var signInModal = SignInModalComponent.WaitAndCreate(Driver);

        signInModal.Login(Configuration.TestEmail, Configuration.TestPassword);

        Driver.Navigate().GoToUrl("https://www.greencity.cx.ua/#/greenCity/news");
        newsPage = new NewsPage(Driver);
    }

    [Test]
    [Description("Filter news by tags - verify each tag shows relevant news and counter updates correctly")]
    [Category("Smoke")]
    public void FilterNewsByTags_ShowsOnlyNewsWithThatTag()
    {
        Assert.That(newsPage, Is.Not.Null, "NewsPage was not initialized");

        var allTags = newsPage.TagsFilter.GetAllAvailableTags();
        Assert.That(allTags, Is.Not.Empty, "No tags found on the page");

        for (int i = 0; i < allTags.Count; i++)
        {
            var tag = allTags[i];

            newsPage.TagsFilter.SelectTagWithRealClick(tag);

            bool isSelected = newsPage.TagsFilter.IsTagSelected(tag);

            newsPage.List.WaitForCardsToLoad();
            var displayedCards = newsPage.List.GetAllNewsCardsAsComponents();

            Assert.That(displayedCards, Is.Not.Empty,
                $"No cards displayed after filtering by '{tag}'");

            foreach (var card in displayedCards)
            {
                var cardTags = card.GetTags();
                Assert.That(cardTags, Does.Contain(tag.ToUpper()),
                    $"Card tags [{string.Join(", ", cardTags)}] do not contain '{tag}'");
            }
            newsPage.TagsFilter.SelectTagWithRealClick(tag);
        }
    }

    [Test]
    [AllureIssue("29")]
    [AllureDescription("Search news by keyword - verify that only news containing the keyword are displayed and the counter updates")]
    [Category("Smoke")]
    public void SearchNewsByKeyword_ShowsOnlyNewsWithKeyword()
    {
        string keyword = "climate";
        Assert.That(newsPage, Is.Not.Null, "NewsPage was not initialized");

        int countBefore = newsPage.GetItemsFoundCount();

        newsPage.Search(keyword);

        newsPage.WaitForSearchResultsToUpdate(countBefore);

        var displayedCards = newsPage.List.GetAllNewsCardsAsComponents();

        Assert.That(displayedCards, Is.Not.Empty,
            $"At least one news card should be displayed after searching for '{keyword}'");

        foreach (var card in displayedCards)
        {
            string title = card.GetTitle();
            Assert.That(title, Does.Contain(keyword).IgnoreCase,
                $"The news title '{title}' does not contain the keyword '{keyword}'");
        }

        int itemsFound = newsPage.GetItemsFoundCount();
        Assert.That(itemsFound, Is.GreaterThan(0),
            $"Items found count should be greater than 0 after search");
    }
}