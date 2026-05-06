using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SeleniumExtras.WaitHelpers;
using green_city_sh.Tests.Components;
using Allure.NUnit.Attributes;

namespace green_city_sh.Tests.Pages;

public class CreateUpdateEventPage : BasePage
{
    // --- Constants for Dynamic Locators and Class Names ---
    private const string InitiativeTypeXPathFormat = "//mat-chip-option[.//span[normalize-space(text()) = '{0}']]";
    private const string GreencityPictureItemCssFormat = ".images-def-wrapper .img-container:nth-child({0}) img";
    private const string ActiveInitiativeTypeClass = "mat-mdc-chip-selected";
    private const string ClassAttribute = "class";
    private const string CreateEventUrlSubstring = "/events/create-update-event";

    // --- Locators ---
    private By HeaderSignInButton => By.XPath("//*[contains(@class, 'sign-in') or contains(text(), 'Sign in')]");
    private By MyEventsTab => By.XPath("//div[@role='tab' and .//span[contains(text(), 'My Events')]]");
    private By AddEventButton => By.Id("create-button-event");

    private By TitleField => By.CssSelector("input[formcontrolname='title']");
    private By DurationDropdown => By.CssSelector("mat-select[formcontrolname='duration']");
    private By EventTypeDropdown => By.CssSelector("mat-select[formcontrolname='open']");
    private By InviteDropdown => By.XPath("//mat-form-field[.//mat-label[contains(text(), 'Invite')]]//mat-select");
    private By DescriptionEditor => By.CssSelector("quill-editor[formcontrolname='description'] .ql-editor");

    private By AddPictureButton => By.CssSelector("input[type='file']");
    private By UploadedImagePreview => By.CssSelector(".input-image-wrapper.selected img");
    private By ClosePictureIcon => By.CssSelector(".selected-delete");
    private By PictureCounter => By.XPath("//div[contains(@class, 'justify-content-between') and .//mat-label[normalize-space()='Picture']]//mat-label[contains(@class, 'xs-text')]");
    private By MainBadge => By.CssSelector(".selected-text");

    private By DatePickerInput => By.XPath("//div[contains(@class, 'mat-mdc-text-field-wrapper') and .//input[@formcontrolname='day']]");
    private By StartTimeInput => By.CssSelector("input[formcontrolname='startTime']");
    private By EndTimeInput => By.CssSelector("input[formcontrolname='finishTime']");
    private By AllDayCheckbox => By.CssSelector("mat-checkbox[formcontrolname='allDay']");
    private By ApplyToAllDaysCheckbox => By.XPath("//mat-checkbox[contains(., 'Apply to all days')]");

    private By PlaceLocationCheckbox => By.XPath("//mat-checkbox[contains(., 'Place')]");
    private By AddressField => By.CssSelector("input[formcontrolname='place']");
    private By FirstAddressSuggestion => By.CssSelector(".pac-container .pac-item");

    private By OnlineLocationCheckbox => By.XPath("//mat-checkbox[contains(., 'Online')]");
    private By OnlineLinkField => By.CssSelector("input[formcontrolname='onlineLink']");

    private By CancelLink => By.CssSelector("button.tertiary-global-button");
    private By PreviewButton => By.XPath("//button[contains(., 'Preview')]");
    private By PublishButton => By.XPath("//button[contains(., 'Publish')]");
    private By BackToEditingButton => By.XPath("//*[contains(text(), 'Back to editing')]");
    private By SuccessSnackBar => By.CssSelector("snack-bar-container, mat-snack-bar-container, .mat-mdc-snack-bar-container");

    // --- Component Wrappers ---
    private DropDownComponent DurationDropdownComp => new(driver, DurationDropdown);
    private DropDownComponent EventTypeDropdownComp => new(driver, EventTypeDropdown);
    private DropDownComponent InviteDropdownComp => new(driver, InviteDropdown);
    private DateTimePickerComponent DatePickerComp => new(driver, DatePickerInput);
    private MaterialCheckboxComponent AllDayCheckboxComp => new(driver, AllDayCheckbox);
    private MaterialCheckboxComponent ApplyToAllDaysCheckboxComp => new(driver, ApplyToAllDaysCheckbox);
    private MaterialCheckboxComponent PlaceLocationCheckboxComp => new(driver, PlaceLocationCheckbox);
    private MaterialCheckboxComponent OnlineLocationCheckboxComp => new(driver, OnlineLocationCheckbox);

    public CreateUpdateEventPage(IWebDriver driver) : base(driver)
    {
    }

    // --- Navigation & Wait Methods ---

    [AllureStep("Click header sign-in button")]
    public void ClickHeaderSignInButton()
    {
        wait.Until(ExpectedConditions.ElementToBeClickable(HeaderSignInButton)).Click();
    }

    [AllureStep("Wait for login modal to disappear")]
    public void WaitForLoginModalToDisappear()
    {
        wait.Until(ExpectedConditions.InvisibilityOfElementLocated(SignInModalComponent.RootLocator));
    }

    [AllureStep("Wait for URL to contain substring")]
    public bool WaitForUrlToContain(string urlSubstring)
    {
        try
        {
            return wait.Until(ExpectedConditions.UrlContains(urlSubstring));
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }

    [AllureStep("Wait for URL to change")]
    public bool WaitForUrlToChange(string previousUrl)
    {
        try
        {
            return wait.Until(d => !string.Equals(d.Url, previousUrl, StringComparison.OrdinalIgnoreCase));
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }

    [AllureStep("Navigate to create event page from profile")]
    public void NavigateToCreateEventPageFromProfile()
    {
        wait.Until(ExpectedConditions.ElementToBeClickable(MyEventsTab)).Click();
        wait.Until(ExpectedConditions.ElementToBeClickable(AddEventButton)).Click();
        wait.Until(ExpectedConditions.UrlContains(CreateEventUrlSubstring));
    }

    // --- Action Methods: General Info ---

    [AllureStep("Set event title")]
    public void SetTitle(string title)
    {
        var input = wait.Until(ExpectedConditions.ElementIsVisible(TitleField));
        input.Clear();
        input.SendKeys(title);
    }

    [AllureStep("Get event title")]
    public string GetTitleValue()
    {
        return wait.Until(ExpectedConditions.ElementIsVisible(TitleField)).GetAttribute("value") ?? string.Empty;
    }

    [AllureStep("Get duration text")]
    public string GetDurationText()
    {
        return DurationDropdownComp.GetSelectedOptionText() ?? string.Empty;
    }

    [AllureStep("Select initiative type")]
    public void SelectInitiativeType(string type)
    {
        By locator = By.XPath(string.Format(InitiativeTypeXPathFormat, type));
        wait.Until(ExpectedConditions.ElementToBeClickable(locator)).Click();
    }

    [AllureStep("Check if initiative type is active")]
    public bool IsInitiativeTypeActive(string type)
    {
        By locator = By.XPath(string.Format(InitiativeTypeXPathFormat, type));
        var element = wait.Until(ExpectedConditions.ElementIsVisible(locator));
        return element.GetAttribute(ClassAttribute).Contains(ActiveInitiativeTypeClass);
    }

    [AllureStep("Select event type")]
    public void SelectEventType(string type)
    {
        EventTypeDropdownComp.Click();
        EventTypeDropdownComp.ClickDropDownOptionByPartialName(type);
    }

    [AllureStep("Get event type text")]
    public string GetEventTypeText()
    {
        return EventTypeDropdownComp.GetSelectedOptionText();
    }

    [AllureStep("Select invite option")]
    public void SelectInviteOption(string invite)
    {
        InviteDropdownComp.Click();
        InviteDropdownComp.ClickDropDownOptionByPartialName(invite);
    }

    [AllureStep("Get invite text")]
    public string GetInviteText()
    {
        return InviteDropdownComp.GetSelectedOptionText();
    }

    [AllureStep("Set event description")]
    public void SetDescription(string text)
    {
        var input = wait.Until(ExpectedConditions.ElementIsVisible(DescriptionEditor));
        input.Clear();
        input.SendKeys(text);
    }

    [AllureStep("Get event description")]
    public string GetDescriptionText()
    {
        return wait.Until(ExpectedConditions.ElementIsVisible(DescriptionEditor)).Text;
    }

    [AllureStep("Upload image")]
    public void UploadImage(string absolutePath)
    {
        wait.Until(ExpectedConditions.ElementExists(AddPictureButton)).SendKeys(absolutePath);
    }

    [AllureStep("Scroll to picture section")]
    public void ScrollToPictureSection()
    {
        var element = wait.Until(ExpectedConditions.ElementExists(AddPictureButton));
        new Actions(driver).ScrollToElement(element).Perform();
    }

    [AllureStep("Select GreenCity picture")]
    public void SelectGreencityPicture(int index)
    {
        By locator = By.CssSelector(string.Format(GreencityPictureItemCssFormat, index));
        wait.Until(ExpectedConditions.ElementToBeClickable(locator)).Click();
    }

    [AllureStep("Check if uploaded image preview is displayed")]
    public bool IsUploadedImagePreviewDisplayed()
    {
        return wait.Until(ExpectedConditions.ElementIsVisible(UploadedImagePreview)).Displayed;
    }

    [AllureStep("Check if main badge is displayed")]
    public bool IsMainBadgeDisplayed()
    {
        return wait.Until(ExpectedConditions.ElementIsVisible(MainBadge)).Displayed;
    }

    [AllureStep("Check if close picture icon is displayed")]
    public bool IsClosePictureIconDisplayed()
    {
        return wait.Until(ExpectedConditions.ElementIsVisible(ClosePictureIcon)).Displayed;
    }

    [AllureStep("Get picture counter text")]
    public string GetPictureCounterText()
    {
        return wait.Until(ExpectedConditions.ElementIsVisible(PictureCounter)).Text;
    }

    // --- Action Methods: Schedule & Location ---

    [AllureStep("Set date")]
    public void SetDate(string date)
    {
        DatePickerComp.EnterDate(date);
    }

    [AllureStep("Get selected date")]
    public string GetSelectedDate()
    {
        return DatePickerComp.GetSelectedDate();
    }

    [AllureStep("Set start and end time")]
    private void SetTimeByJS(By locator, string timeValue)
    {
        var timeInput = wait.Until(ExpectedConditions.ElementToBeClickable(locator));
        var jsExecutor = (IJavaScriptExecutor)driver;

        string script = @"
            var element = arguments[0];
            var value = arguments[1];
            element.value = value;
            element.dispatchEvent(new Event('input', { bubbles: true }));
            element.dispatchEvent(new Event('change', { bubbles: true }));
            element.dispatchEvent(new Event('blur'));
        ";

        jsExecutor.ExecuteScript(script, timeInput, timeValue);
    }

    [AllureStep("Set time")]
    public void SetTime(string start, string end)
    {
        SetTimeByJS(StartTimeInput, start);
        SetTimeByJS(EndTimeInput, end);
    }

    [AllureStep("Set start time")]
    public void SetStartTime(string time) => SetTimeByJS(StartTimeInput, time);

    [AllureStep("Set end time")]
    public void SetEndTime(string time) => SetTimeByJS(EndTimeInput, time);

    [AllureStep("Get start time value")]
    public string GetStartTimeValue() => wait.Until(ExpectedConditions.ElementIsVisible(StartTimeInput)).GetAttribute("value") ?? string.Empty;

    [AllureStep("Get end time value")]
    public string GetEndTimeValue() => wait.Until(ExpectedConditions.ElementIsVisible(EndTimeInput)).GetAttribute("value") ?? string.Empty;

    [AllureStep("Check all day")]
    public void CheckAllDay() => AllDayCheckboxComp.Check();

    [AllureStep("Check apply to all days")]
    public void CheckApplyToAllDays() => ApplyToAllDaysCheckboxComp.Check();

    [AllureStep("Is apply to all days checked")]
    public bool IsApplyToAllDaysChecked() => ApplyToAllDaysCheckboxComp.IsChecked();

    [AllureStep("Select place location")]
    public void SelectPlaceLocation()
    {
        PlaceLocationCheckboxComp.Check();
    }

    [AllureStep("Is place location checked")]
    public bool IsPlaceLocationChecked() => PlaceLocationCheckboxComp.IsChecked();

    [AllureStep("Enter address")]
    public void EnterAddress(string address)
    {
        var input = wait.Until(ExpectedConditions.ElementIsVisible(AddressField));
        input.Clear();
        input.SendKeys(address);
    }

    [AllureStep("Select address from dropdown")]
    public void SelectAddressFromDropdown()
    {
        var js = (IJavaScriptExecutor)driver;
        js.ExecuteScript("document.activeElement.blur();");

        var input = wait.Until(ExpectedConditions.ElementToBeClickable(AddressField));
        input.Click();

        var suggestion = wait.Until(ExpectedConditions.ElementToBeClickable(FirstAddressSuggestion));
        suggestion.Click();
        input.SendKeys(Keys.Enter);

        // Wait for input to be populated instead of Thread.Sleep
        wait.Until(d => !string.IsNullOrEmpty(d.FindElement(AddressField).GetAttribute("value")));
    }

    [AllureStep("Select online location")]
    public void SelectOnlineLocation(string url)
    {
        OnlineLocationCheckboxComp.Check();
        SetOnlineLink(url);
    }

    [AllureStep("Check online location")]
    public void CheckOnlineLocation() => OnlineLocationCheckboxComp.Check();

    [AllureStep("Is online location checked")]
    public bool IsOnlineLocationChecked() => OnlineLocationCheckboxComp.IsChecked();

    [AllureStep("Set online link")]
    public void SetOnlineLink(string url)
    {
        var input = wait.Until(ExpectedConditions.ElementIsVisible(OnlineLinkField));
        input.Clear();
        input.SendKeys(url);
    }

    [AllureStep("Get online link value")]
    public string GetOnlineLinkValue() => wait.Until(ExpectedConditions.ElementIsVisible(OnlineLinkField)).GetAttribute("value") ?? string.Empty;

    // --- Action Methods: Buttons ---

    [AllureStep("Click publish")]
    public void ClickPublish()
    {
        wait.Until(ExpectedConditions.ElementToBeClickable(PublishButton)).Click();
    }

    [AllureStep("Click preview")]
    public void ClickPreview()
    {
        wait.Until(ExpectedConditions.ElementToBeClickable(PreviewButton)).Click();
    }

    [AllureStep("Click cancel")]
    public void ClickCancel()
    {
        wait.Until(ExpectedConditions.ElementToBeClickable(CancelLink)).Click();
    }

    [AllureStep("Click back to editing")]
    public void ClickBackToEditing()
    {
        wait.Until(ExpectedConditions.ElementToBeClickable(BackToEditingButton)).Click();
    }

    [AllureStep("Hover over publish button")]
    public void HoverOverPublishButton()
    {
        var element = wait.Until(ExpectedConditions.ElementIsVisible(PublishButton));
        new Actions(driver).MoveToElement(element).Perform();
    }

    [AllureStep("Is publish button enabled")]
    public bool IsPublishButtonEnabled()
    {
        return wait.Until(ExpectedConditions.ElementExists(PublishButton)).Enabled;
    }

    [AllureStep("Is success snackbar displayed")]
    public bool IsSuccessSnackBarDisplayed()
    {
        return wait.Until(ExpectedConditions.ElementIsVisible(SuccessSnackBar)).Displayed;
    }
}
