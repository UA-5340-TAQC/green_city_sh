using OpenQA.Selenium;
using System.Globalization;
using SeleniumExtras.WaitHelpers;

namespace green_city_sh.Tests.Components;

public class EventsFilterSectionComponent : BaseComponent
{
    private By EventTimeDropdown => By.CssSelector("#mat-select-8"); //повертає дропдаун для вибору часу проведення заходу (Event time)
    private By LocationDropdown => By.CssSelector("#mat-select-10"); //повертає дропдаун для вибору місця проведення заходу (Location)
    private By StatusDropdown => By.CssSelector("#mat-select-12"); //повертає дропдаун для вибору статусу заходу (Status)
    private By TypeDropdown => By.XPath("//div[contains(@class, 'dropdown')][./mat-label[contains(text(), 'Type')]]//mat-select");
    private By TypeOption(string option) => By.XPath($"//mat-option//span[contains(text(), '{option}')]"); 
    private By DateRangeDropdown => By.CssSelector(".date-range-input + .mat-mdc-select-arrow-wrapper");    
    private By nextBtn = By.CssSelector(".mat-calendar-next-button");
    private By prevBtn = By.CssSelector(".mat-calendar-previous-button");
    private By periodBtn = By.CssSelector(".mat-calendar-period-button");
    private By DayButton(string label) => By.XPath($"//button[@aria-label='{label}']");
    
    public EventsFilterSectionComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public EventsFilterSectionComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }

    private void PickDate(DateTime date)
    {
        var culture = new CultureInfo("en-US");
        int targetMonth = date.Month;
        int targetYear = date.Year;
        
        while (true)
        {
            var periodButton = driver.FindElement(periodBtn);
            string text = periodButton.Text;
            if (string.IsNullOrEmpty(text))
            {
                continue;
            }
            var parts = text.Split(' ');
            if (parts.Length < 2)
                continue;

            string monthText = parts[0];
            int year = int.Parse(parts[1]);

            int month = DateTime.ParseExact(monthText, "MMMM", culture).Month;
            if (month == targetMonth && year == targetYear)
                break;
            
            if (year < targetYear || (year == targetYear && month < targetMonth))
            {
                driver.FindElement(nextBtn).Click();
            }
            else
            {
                driver.FindElement(prevBtn).Click();
            }
        }
        string label = date.ToString("MMMM d, yyyy", culture);

        var day = driver.FindElement(DayButton(label));
        day.Click();
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

    public bool IsSelectTypeDisplayed()
    {
        return RootElement.FindElement(TypeDropdown).Displayed;
    }

    public bool isSelectTypeEnabled()
    {
        return RootElement.FindElement(TypeDropdown).Enabled;
    }

    public void ClickSelectTypeDropdown()
    {
        RootElement.FindElement(TypeDropdown).Click();
    }
    public void SelectType(string option)
    {
        var optionElement = driver.FindElement(TypeOption(option));
        optionElement.Click();
    }

    public void CloseSelectTypeDropdown()
    {
        new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();
    }
    public bool IsDateRangeEnabled()
    {
        return driver.FindElement(DateRangeDropdown).Enabled;
    }
    
    public void ClickDateRangeDropdown()
    {
        var arrow = wait.Until(ExpectedConditions.ElementToBeClickable(DateRangeDropdown));
        arrow.Click();
    }

    public void SelectDateRange(DateTime start, DateTime end)
    {
        PickDate(start);
        PickDate(end);
    }

    

    public void ClickResetAll()
    {
        //Клікнути на кнопку "Reset all"
    }
}
