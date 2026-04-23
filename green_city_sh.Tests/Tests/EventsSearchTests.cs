using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;

namespace green_city_sh.Tests.Tests;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class EventsSearchTests : BaseTest
{
    private EventsPage? eventsPage;

    [SetUp]
    public void SetUp()
    {
        NavigateToBaseUrl();

        eventsPage = new EventsPage(Driver!);
        eventsPage.GlobalHeader.ChangeLanguage("en");
        eventsPage.GlobalHeader.ClickSignIn();
        var signInModal = SignInModalComponent.WaitAndCreate(Driver!);
        signInModal.Login("greencitytest69@hotmail.com", "asweQA5346!)");
        Thread.Sleep(2000); // Temporary solution before adding proper wait for login method
        eventsPage.OpenEventsPage();
    }

    [Test]
    [Category("Smoke")]
    public void SearchByKeywordReturnsMatchingEvents()
    {
        string searchKeyword = "2026";

        eventsPage!.EventsTopBar.ClickSearchIcon();
        eventsPage.EventsTopBar.FillSearchInputField(searchKeyword);
        eventsPage.EventList.WaitForCardsToLoad(); 

        var searchResults = eventsPage.GetAllEventCards();

        Assert.That(searchResults, Is.Not.Empty, 
            $"Expected to find at least one event matching the keyword '{searchKeyword}'.");

        foreach (var card in searchResults)
        {
            string title = card.GetTitle().ToLower();
            
            bool isRelevant = title.Contains(searchKeyword.ToLower());

            Assert.That(isRelevant, Is.True, 
                $"Event card '{title}' did not contain the search keyword '{searchKeyword}'.");
        }
    }
}
