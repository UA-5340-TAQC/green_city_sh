using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;

namespace green_city_sh.Tests.Tests;

[TestFixture]
public class SearchTypeAndDateRangeTest : BaseTest
{
    private EventsPage? eventsPage;
    
    protected override void OnSetup()
    {
        NavigateToBaseUrl();
        
        eventsPage = new EventsPage(Driver!);
        eventsPage.Header.ChangeLanguage("en");
        
    }
    
    [Test]
    [Category("Smoke")]
    public void SearchTypeAndDateRange()
    {
        eventsPage.OpenEventsPage();
        
        Assert.That(eventsPage.EventsTopBar.IsSearchIconVisible(), "Search icon should be visible on the events page.");
        Assert.That(eventsPage.EventsTopBar.IsSearchIconEnabled(), "Search button should be visible on the events page.");
        eventsPage!.ClickSearchButton();
        eventsPage.EventsTopBar.FillSearchInputField("Community");
        var searchResults=new List<EventCardComponent>();
        if (eventsPage.EventsTopBar.IsSearchIconEnabled())
        {
            searchResults = eventsPage.EventList.GetAllEventCards();
        }
        Assert.That(searchResults.Count, Is.GreaterThan(0), "Search results should be displayed more, than 0.");
        
        if (searchResults.Count > 0)
        {
            foreach (var card in searchResults)
            {
                string title = card.GetTitle();
                bool isRelevant = title.Contains("Community", StringComparison.OrdinalIgnoreCase);

                Assert.That(isRelevant, Is.True,
                    $"Event card '{title}' did not contain the search keyword 'Community'.");
            }
        }
        
        Assert.That(eventsPage.FilterSection.IsSelectTypeDisplayed(), "the SelectType dropdown isn't displayed");
        Assert.That(eventsPage.FilterSection.isSelectTypeEnabled(), "the SelectType dropdown isn't enabled");  
        
        eventsPage.FilterSection.ClickSelectTypeDropdown();
        eventsPage.FilterSection.SelectType("Economic");
        var typeResults = eventsPage.EventList.GetAllEventCards();
        Assert.That(typeResults.Count, Is.GreaterThan(0), "Type results should be displayed more, than 0.");
        
        eventsPage.FilterSection.CloseSelectTypeDropdown();
        Assert.That(eventsPage.FilterSection.IsDateRangeEnabled(), "the Date Range filter isn't enabled");
        eventsPage.FilterSection.ClickDateRangeDropdown();
        
        var currentDate = DateTime.Today;
        var nextDay = currentDate.AddDays(7);

        eventsPage.FilterSection.SelectDateRange(currentDate, nextDay);
        var filteredResults = eventsPage.EventList.GetAllEventCards();
        Assert.That(filteredResults.Count, Is.GreaterThan(0), "Filtered results should be displayed more, than 0.");
        
    }
}
