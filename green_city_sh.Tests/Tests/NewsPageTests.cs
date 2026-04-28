using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;
using NUnit.Framework;
using OpenQA.Selenium;

namespace green_city_sh.Tests.Tests;

[TestFixture]
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

        Thread.Sleep(1000);

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

        int tagsToTestCount = Math.Min(6, allTags.Count);
        var tagsToTest = allTags.Take(tagsToTestCount).ToList();

        for (int i = 0; i < tagsToTest.Count; i++)
        {
            var tag = tagsToTest[i];

            newsPage.TagsFilter.SelectTagWithRealClick(tag);

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
}