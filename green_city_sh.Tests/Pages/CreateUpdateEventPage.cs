using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using green_city_sh.Tests.Components;

namespace green_city_sh.Tests.Pages;

public class CreateUpdateEventPage : BasePage
{
    // --- Constants for Dynamic Locators and Class Names ---
    // Updated to use exact normalized-text matching to prevent false positive chip selections
    private const string InitiativeTypeXPathFormat = "//mat-chip-option[.//span[normalize-space(text()) = '{0}']]";
    private const string GreencityPictureItemCssFormat = ".images-def-wrapper .img-container:nth-child({0}) img";
    private const string ActiveInitiativeTypeClass = "mat-mdc-chip-selected";
    private const string ClassAttribute = "class";
    private const string CreateEventUrlSubstring = "/events/create-update-event";

    // --- Locators ---
    private readonly By _headerSignInButtonLocator = By.XPath("//*[contains(@class, 'sign-in') or contains(text(), 'Sign in')]");
    private readonly By _myEventsTabLocator = By.XPath("//div[@role='tab' and .//span[contains(text(), 'My Events')]]");
    private readonly By _addEventButtonLocator = By.Id("create-button-event");
    
    private readonly By _titleFieldLocator = By.CssSelector("input[formcontrolname='title']");
    private readonly By _durationDropdownLocator = By.CssSelector("mat-select[formcontrolname='duration']");
    private readonly By _eventTypeDropdownLocator = By.CssSelector("mat-select[formcontrolname='open']");
    private readonly By _inviteDropdownLocator = By.XPath("//mat-form-field[.//mat-label[contains(text(), 'Invite')]]//mat-select");
    private readonly By _descriptionEditorLocator = By.CssSelector("quill-editor[formcontrolname='description'] .ql-editor");
    private readonly By _addPictureButtonLocator = By.CssSelector("input[type='file']");
    private readonly By _uploadedImagePreviewLocator = By.CssSelector(".input-image-wrapper.selected img");
    private readonly By _closePictureIconLocator = By.CssSelector(".selected-delete");
    //Precise locator targeting the specific mat-label inside the Picture section header
    private readonly By _pictureCounterLocator = By.XPath("//div[contains(@class, 'justify-content-between') and .//mat-label[normalize-space()='Picture']]//mat-label[contains(@class, 'xs-text')]");
    private readonly By _mainBadgeLocator = By.CssSelector(".selected-text");
    
    private readonly By _datePickerLocator = By.XPath("//div[contains(@class, 'mat-mdc-text-field-wrapper') and .//input[@formcontrolname='day']]");
    private readonly By _startTimeInputLocator = By.CssSelector("input[formcontrolname='startTime']");
    private readonly By _endTimeInputLocator = By.CssSelector("input[formcontrolname='finishTime']");
    private readonly By _allDayCheckboxLocator = By.CssSelector("mat-checkbox[formcontrolname='allDay']");
    private readonly By _applyToAllDaysCheckboxLocator = By.XPath("//mat-checkbox[contains(., 'Apply to all days')]");
    private readonly By _placeLocationCheckboxLocator = By.XPath("//mat-checkbox[contains(., 'Place')]");
    private readonly By _onlineLocationCheckboxLocator = By.XPath("//mat-checkbox[contains(., 'Online')]");
    private readonly By _onlineLinkFieldLocator = By.CssSelector("input[formcontrolname='onlineLink']");
    
    private readonly By _cancelLinkLocator = By.CssSelector("button.tertiary-global-button");
    private readonly By _previewButtonLocator = By.XPath("//button[contains(., 'Preview')]");
    private readonly By _publishButtonLocator = By.XPath("//button[contains(., 'Publish')]");
    private readonly By _successSnackBarLocator = By.CssSelector("snack-bar-container, mat-snack-bar-container, .mat-mdc-snack-bar-container");

    public CreateUpdateEventPage(IWebDriver driver) : base(driver)
    {
    }

    // --- Wait Encapsulations & Navigation Methods ---

    public void ClickHeaderSignInButton()
    {
        wait.Until(ExpectedConditions.ElementToBeClickable(_headerSignInButtonLocator)).Click();
    }

    public void WaitForLoginModalToDisappear()
    {
        wait.Until(ExpectedConditions.InvisibilityOfElementLocated(SignInModalComponent.RootLocator));
    }

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

    public void NavigateToCreateEventPageFromProfile()
    {
        wait.Until(ExpectedConditions.ElementToBeClickable(_myEventsTabLocator)).Click();
        wait.Until(ExpectedConditions.ElementToBeClickable(_addEventButtonLocator)).Click();
        wait.Until(ExpectedConditions.UrlContains(CreateEventUrlSubstring));
    }

    // --- Left Column (General Info) ---

    public IWebElement TitleField => wait.Until(ExpectedConditions.ElementIsVisible(_titleFieldLocator));

    public DropDownComponent DurationDropdown => new(driver, _durationDropdownLocator);

    public IWebElement InitiativeTypeButton(string type)
    {
        By locator = By.XPath(string.Format(InitiativeTypeXPathFormat, type));
        return wait.Until(ExpectedConditions.ElementToBeClickable(locator));
    }

    public bool IsInitiativeTypeActive(string type)
    {
        By locator = By.XPath(string.Format(InitiativeTypeXPathFormat, type));
        IWebElement element = wait.Until(ExpectedConditions.ElementIsVisible(locator));
        return element.GetAttribute(ClassAttribute).Contains(ActiveInitiativeTypeClass);
    }

    public DropDownComponent EventTypeDropdown => new(driver, _eventTypeDropdownLocator);

    public DropDownComponent InviteDropdown => new(driver, _inviteDropdownLocator);

    public IWebElement DescriptionEditor => wait.Until(ExpectedConditions.ElementIsVisible(_descriptionEditorLocator));

    public IWebElement AddPictureButton => wait.Until(ExpectedConditions.ElementExists(_addPictureButtonLocator));

    public IWebElement UploadedImagePreview => wait.Until(ExpectedConditions.ElementIsVisible(_uploadedImagePreviewLocator));

    public IWebElement ClosePictureIcon => wait.Until(ExpectedConditions.ElementToBeClickable(_closePictureIconLocator));

    public IWebElement PictureCounter => wait.Until(ExpectedConditions.ElementIsVisible(_pictureCounterLocator));

    public IWebElement MainBadge => wait.Until(ExpectedConditions.ElementIsVisible(_mainBadgeLocator));

    public IWebElement GreencityPictureItem(int index)
    {
        By locator = By.CssSelector(string.Format(GreencityPictureItemCssFormat, index));
        return wait.Until(ExpectedConditions.ElementToBeClickable(locator));
    }

    // --- Right Column (Schedule & Location) ---

    public DateTimePickerComponent DatePicker => new(driver, _datePickerLocator);

    public IWebElement StartTimeInput => wait.Until(ExpectedConditions.ElementIsVisible(_startTimeInputLocator));

    public IWebElement EndTimeInput => wait.Until(ExpectedConditions.ElementIsVisible(_endTimeInputLocator));

    public MaterialCheckboxComponent AllDayCheckbox => new(driver, _allDayCheckboxLocator);

    public MaterialCheckboxComponent ApplyToAllDaysCheckbox => new(driver, _applyToAllDaysCheckboxLocator);

    public MaterialCheckboxComponent PlaceLocationCheckbox => new(driver, _placeLocationCheckboxLocator);

    public MaterialCheckboxComponent OnlineLocationCheckbox => new(driver, _onlineLocationCheckboxLocator);

    public IWebElement OnlineLinkField => wait.Until(ExpectedConditions.ElementIsVisible(_onlineLinkFieldLocator));

    // --- Actions / Notifications ---

    public IWebElement CancelLink => wait.Until(ExpectedConditions.ElementToBeClickable(_cancelLinkLocator));

    public IWebElement PreviewButton => wait.Until(ExpectedConditions.ElementToBeClickable(_previewButtonLocator));

    public IWebElement PublishButton => wait.Until(ExpectedConditions.ElementToBeClickable(_publishButtonLocator));

    public IWebElement SuccessSnackBar => wait.Until(ExpectedConditions.ElementIsVisible(_successSnackBarLocator));

    public void SetTimeByJS(IWebElement timeInput, string timeValue)
    {
        // Ensure the element is ready for interaction
        wait.Until(ExpectedConditions.ElementToBeClickable(timeInput));

        var jsExecutor = (IJavaScriptExecutor)driver;
        
        // JavaScript to set the value and trigger Angular's formControl listeners
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
}
