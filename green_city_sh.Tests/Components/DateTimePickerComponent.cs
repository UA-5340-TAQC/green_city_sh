using System;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using green_city_sh.Tests.Infrastructure;

namespace green_city_sh.Tests.Components;

public class DateTimePickerComponent : BaseComponent
{
    private readonly By _inputLocator = By.CssSelector("input.mat-datepicker-input");
    private readonly By _calendarToggleLocator = By.CssSelector("mat-datepicker-toggle button");

    public DateTimePickerComponent(IWebDriver driver, By rootLocator) 
        : base(driver, rootLocator)
    {
    }

    public DateTimePickerComponent(IWebDriver driver, IWebElement componentRoot) 
        : base(driver, componentRoot)
    {
    }

    public void EnterDate(string date)
    {
        if (string.IsNullOrWhiteSpace(date))
            throw new ArgumentException("Date cannot be null or empty.", nameof(date));

        var element = wait.Until(d => RootElement.FindElement(_inputLocator));
        wait.Until(ExpectedConditions.ElementToBeClickable(element));

        element.SendKeys(Keys.Control + "a");
        element.SendKeys(Keys.Backspace);

        // Enter the new date
        element.SendKeys(date);
    }

    public void OpenCalendar()
    {
        WaitAndClick(_calendarToggleLocator);
    }

    public string GetSelectedDate()
    {
        return FindElement(_inputLocator).GetAttribute("value");
    }
}
