using System.Text.RegularExpressions;
using green_city_sh.Tests.Components;
using green_city_sh.Tests.Modals;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace green_city_sh.Tests.Pages;

public class SavedPage : BasePage
{
    private BookmarkTabComponent? bookmarkTab;
    private EventsListComponent? eventsList;

    public SavedPage(IWebDriver driver) : base(driver)
    {
    }

    public BookmarkTabComponent BookmarkTab => bookmarkTab ??= new BookmarkTabComponent(driver, By.CssSelector(".tabs"));

    public EventsListComponent EventsList => eventsList ??= new EventsListComponent(driver, By.CssSelector(".event-list"));
}