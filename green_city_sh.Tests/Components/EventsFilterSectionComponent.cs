using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class EventsFilterSectionComponent : BaseComponent
{
    private By EventTimeDropdown => By.CssSelector("#mat-select-8"); //повертає дропдаун для вибору часу проведення заходу (Event time)
    private By LocationDropdown => By.CssSelector("#mat-select-10"); //повертає дропдаун для вибору місця проведення заходу (Location)
    private By StatusDropdown => By.CssSelector("#mat-select-12"); //повертає дропдаун для вибору статусу заходу (Status)
    private By TypeDropdown => By.CssSelector("#mat-select-14"); //повертає дропдаун для вибору типу заходу (Type)
    private By DateRangeDropdown => By.CssSelector("#mat-date-range-input-1"); //повертає дропдаун для вибору діапазону дат проведення заходу (Date range)
    private By ResetAllButton => By.CssSelector(".reset"); //повертає кнопку "Reset all" для скидання всіх вибраних фільтрів

    public EventsFilterSectionComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public EventsFilterSectionComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }

    public void SelectEventTime(string option)
    {
        //Вибрати опцію в дропдауні "Event time" ("Any time", "Upcoming", "Past")
    }

    public void SelectLocation(string option)
    {
        //Вибрати опцію в дропдауні "Location" ("Select All", "Online", "Offline", not "Filter cities")
    }

    public void SelectStatus(string option)
    {
        //Вибрати опцію в дропдауні "Status" ("Any status", "Open", "Closed", "Joined", "Created", "Saved")
    }

    public void SelectType(string option)
    {
        //Вибрати опцію в дропдауні "Type" ("All types", "Economic", "Environmental", "Social")
    }

    public void SelectDateRange(string fromDate, string toDate)
    {
        //Вибрати діапазон дат від "fromDate" до "toDate" в дропдауні "Date range"
    }

    public void ClickResetAll()
    {
        //Клікнути на кнопку "Reset all"
    }
}
