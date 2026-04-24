using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;

namespace green_city_sh.Tests.Tests;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class EventsSearchTests : BaseTest
{
    private EventsPage? eventsPage;

    protected override void OnSetup()
    {
        NavigateToBaseUrl();

        eventsPage = new EventsPage(Driver!);
        eventsPage.Header.ChangeLanguage("en");
        eventsPage.Header.ClickSignIn();
        var signInModal = SignInModalComponent.WaitAndCreate(Driver!);
        signInModal.Login("greencitytest69@hotmail.com", "asweQA5346!)");
        eventsPage.OpenEventsPage();
    }

    [Test]
    [Category("Smoke")]
    [TestCase("2026")]
    public void SearchByKeywordReturnsMatchingEvents(string searchKeyword)
    {
        eventsPage!.EventsTopBar.ClickSearchIcon();
        eventsPage.EventsTopBar.FillSearchInputField(searchKeyword);
        eventsPage.EventList.WaitForCardsToLoad(); 

        var searchResults = eventsPage.GetAllEventCards();

        Assert.That(searchResults, Is.Not.Empty, 
            $"Expected to find at least one event matching the keyword '{searchKeyword}'.");

        foreach (var card in searchResults)
        {
            string title = card.GetTitle();
            bool isRelevant = title.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase );

            Assert.That(isRelevant, Is.True, 
                $"Event card '{title}' did not contain the search keyword '{searchKeyword}'.");
        }
    }
}
