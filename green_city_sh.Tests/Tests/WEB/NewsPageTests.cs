using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;
using OpenQA.Selenium;
using Allure.NUnit.Attributes;

namespace green_city_sh.Tests.Tests.WEB;

[AllureSubSuite("NewsPage")]
[AllureTag("Smoke")]
[AllureFeature("News Filtering", "News Search")]
public class NewsPageTests : BaseUITest
{
    private NewsPage? newsPage;
    private HeaderComponent? header;

    protected override void OnSetup()
    {
        NavigateToBaseUrl();
        header = new HomePage(Driver!).Header;

        header.ChangeLanguage("En");

        var signInModal = header.ClickSignIn();

        signInModal.Login(Configuration.TestEmail, Configuration.TestPassword);

        Driver!.Navigate().GoToUrl($"{Configuration.BaseUrl}/news");
        newsPage = new NewsPage(Driver!);
    }

    [Test]
    [AllureIssue("28")]
    [AllureTag("FilterTest", "News")]
    [AllureFeature("Filter by Tags")]
    [AllureDescription("Filter news by tags - verify each tag shows relevant news and counter updates correctly")]
    [Category("Smoke")]
    public void FilterNewsByTags_ShowsOnlyNewsWithThatTag()
    {
        Assert.That(newsPage, Is.Not.Null, "NewsPage was not initialized");

        var allTags = newsPage.TagsFilter.GetAllAvailableTags();
        Assert.That(allTags, Is.Not.Empty, "No tags found on the page");

        for (int i = 0; i < allTags.Count; i++)
        {
            var tag = allTags[i];
            int countBefore = newsPage.GetItemsFoundCount();

            newsPage.TagsFilter.SelectTagWithRealClick(tag);

            newsPage.List.WaitForCardsToRefresh(countBefore);
            newsPage.WaitForItemsCountToChange(countBefore);

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
            int countAfter = newsPage.GetItemsFoundCount();
            newsPage.List.WaitForCardsToRefresh(countAfter);
            newsPage.WaitForItemsCountToChange(displayedCards.Count);
        }
    }

    [Test]
    [AllureIssue("29")]
    [AllureTag("SearchTest", "News")]
    [AllureFeature("Search by Keyword")]
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

    [Test]
    [AllureIssue("6")]
    [AllureTag("FilterTest", "News")]
    [AllureFeature("Multiple Filters")]
    [AllureDescription("Verify that filter results are displayed correctly when selecting 'Events' and 'Ads'")]
    [Category("Smoke")]
    public void FilterByEventsAndAds_DisplaysCorrectResults()
    {
        Assert.That(newsPage, Is.Not.Null, "NewsPage was not initialized");

        newsPage.ClearAllFilters();
        newsPage.List.WaitForCardsToLoad();

        newsPage.TagsFilter.SelectTagWithRealClick("Events");
        newsPage.List.WaitForCardsToLoad();
        Assert.That(newsPage.TagsFilter.IsTagSelected("Events"), Is.True);

        newsPage.TagsFilter.SelectTagWithRealClick("Ads");
        newsPage.List.WaitForCardsToLoad();

        Assert.That(newsPage.TagsFilter.IsTagSelected("Events"), Is.True);
        Assert.That(newsPage.TagsFilter.IsTagSelected("Ads"), Is.True);

        var displayedCards = newsPage.List.GetAllNewsCardsAsComponents();
        Assert.That(displayedCards, Is.Not.Empty);

        foreach (var card in displayedCards)
        {
            var cardTags = card.GetTags();
            Assert.That(cardTags, Does.Contain("EVENTS"),
                $"Card should contain 'Events' tag. Found: [{string.Join(", ", cardTags)}]");
            Assert.That(cardTags, Does.Contain("ADS"),
                $"Card should contain 'Ads' tag. Found: [{string.Join(", ", cardTags)}]");
        }
    }
}
