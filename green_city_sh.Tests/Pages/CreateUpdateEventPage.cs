using System;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace green_city_sh.Tests.Pages;

public class CreateUpdateEventPage : BasePage
{
    // Left column (General Info)
    private By TitleField => By.CssSelector("input[formcontrolname='title']");
    private By DurationDropdown => By.CssSelector("mat-select[formcontrolname='duration']");
    private By InitiativeTypeButton(string type) => By.XPath($"//button[contains(., '{type}')]");
    private By EventTypeDropdown => By.CssSelector("mat-select[formcontrolname='open']");
    private By InviteDropdown => By.XPath("//mat-form-field[.//mat-label[contains(text(), 'Invite')]]//mat-select");

    private By DescriptionEditor => By.CssSelector("textarea[formcontrolname='description'], .ql-editor");

    private By AddPictureButton => By.CssSelector("input[type='file']");
    private By UploadedImagePreview => By.CssSelector(".image-preview");
    private By ClosePictureIcon => By.CssSelector(".selected-delete");
    private By GreencityPictureItem(int index) => By.CssSelector($".images-container img:nth-child({index}), .gallery-item:nth-child({index})");

    // Right Column (Schedule & Location)
    private By DatePickerInput => By.CssSelector("input[matinput][formcontrolname='day']");
    private By StartTimeInput => By.CssSelector("input[formcontrolname='startTime']");
    private By EndTimeInput => By.CssSelector("input[formcontrolname='finishTime']");
    private By AllDayCheckbox => By.CssSelector("mat-checkbox[formcontrolname='allDay']");

    private By PlaceLocationCheckbox => By.XPath("//div[contains(@class, 'mdc-form-field')][.//label[contains(., 'Place')]]//input");
    private By AddressField => By.CssSelector("input[formcontrolname='place']");
    private By FirstAddressSuggestion => By.CssSelector(".pac-container .pac-item");

    private By OnlineLocationCheckbox => By.XPath("//div[contains(@class, 'mdc-form-field')][.//label[contains(., 'Online')]]//input");
    private By OnlineLinkField => By.CssSelector("input[formcontrolname='onlineLink']");

    private By CancelLink => By.CssSelector(".tertiary-global-button");
    private By PreviewButton => By.XPath("//button[contains(., 'Preview')]");
    private By PublishButton => By.XPath("//button[contains(., 'Publish')]");
    private By BackToEditingButton => By.XPath("//*[contains(text(), 'Back to editing')]");

    public CreateUpdateEventPage(IWebDriver driver) : base(driver)
    {
    }

    private IWebElement WaitAndFindElement(By locator)
    {
        return wait.Until(d => d.FindElement(locator));
    }

    public void SetTitle(string title)
    {
        var input = WaitAndFindElement(TitleField);
        input.Clear();
        input.SendKeys(title);
    }

    public void SelectInitiativeType(string type)
    {
        WaitAndFindElement(InitiativeTypeButton(type)).Click();
    }

    public void SelectEventType(string type)
    {
        WaitAndFindElement(EventTypeDropdown).Click();
        WaitAndFindElement(By.XPath($"//mat-option[contains(., '{type}')]")).Click();
    }

    public void SetDescription(string text)
    {
        var input = WaitAndFindElement(DescriptionEditor);
        input.Clear();
        input.SendKeys(text);
    }

    public void UploadImage(string absolutePath)
    {
        WaitAndFindElement(AddPictureButton).SendKeys(absolutePath);
    }

    public void SelectGreencityPicture(int index)
    {
        WaitAndFindElement(GreencityPictureItem(index)).Click();
    }

    public void SetDate(string date)
    {
        var input = WaitAndFindElement(DatePickerInput);
        input.Clear();
        input.SendKeys(date);
    }

    public void SetTime(string start, string end)
    {
        var startInput = WaitAndFindElement(StartTimeInput);
        startInput.Clear();
        startInput.SendKeys(start);

        var endInput = WaitAndFindElement(EndTimeInput);
        endInput.Clear();
        endInput.SendKeys(end);
    }

    public void SelectPlaceLocation()
    {
        var checkbox = wait.Until(d => d.FindElement(PlaceLocationCheckbox));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", checkbox);
    }

    public void SelectOnlineLocation(string url)
    {
        var checkbox = wait.Until(d => d.FindElement(OnlineLocationCheckbox));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", checkbox);

        var linkInput = WaitAndFindElement(OnlineLinkField);
        linkInput.Clear();
        linkInput.SendKeys(url);
    }

    public void EnterAddress(string address)
    {
        var input = WaitAndFindElement(AddressField);
        input.Clear();
        input.SendKeys(address);
    }

    public void SelectAddressFromDropdown()
    {
        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
        js.ExecuteScript("document.activeElement.blur();");
        var input = wait.Until(ExpectedConditions.ElementToBeClickable(AddressField));
        input.Click();
        var suggestion = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".pac-item")));
        suggestion.Click();
        input.SendKeys(Keys.Enter);
        Thread.Sleep(500);
    }

    public void ClickPublish()
    {
        WaitAndFindElement(PublishButton).Click();
    }

    public void ClickPreview()
    {
        WaitAndFindElement(PreviewButton).Click();
    }

    public void ClickCancel()
    {
        WaitAndFindElement(CancelLink).Click();
    }

    public void ClickBackToEditing()
    {
        WaitAndFindElement(BackToEditingButton).Click();
    }
}