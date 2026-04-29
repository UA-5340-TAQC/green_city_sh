using OpenQA.Selenium;

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
    private By GreencityPictureItem(int index) => By.CssSelector($".images-container img:nth-child({index}), .gallery-item:nth-child({index})"); // a specific picture by index

    // Right Column (Schedule & Location)
    private By DatePickerInput => By.CssSelector("input[matinput][formcontrolname='day']");
    private By StartTimeInput => By.CssSelector("input[formcontrolname='startTime']");
    private By EndTimeInput => By.CssSelector("input[formcontrolname='finishTime']");
    private By AllDayCheckbox => By.CssSelector("mat-checkbox[formcontrolname='allDay']");
    private By PlaceLocationCheckbox => By.Id("mat-mdc-checkbox-2-input");
    private By OnlineLocationCheckbox => By.Id("mat-mdc-checkbox-3-input");
    private By OnlineLinkField => By.CssSelector("input[formcontrolname='onlineLink']");

    private By CancelLink => By.CssSelector(".tertiary-global-button");
    private By PreviewButton => By.XPath("//button[contains(., 'Preview')]");
    private By PublishButton => By.XPath("//button[contains(., 'Publish')]");

    public CreateUpdateEventPage(IWebDriver driver) : base(driver)
    {
    }


    public void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be null or empty.", nameof(title));
    }

    public void SelectInitiativeTypes(params string[] types)
    {
        if (types == null || types.Length == 0)
            throw new ArgumentException("At least one initiative type must be provided.", nameof(types));
    }

    public void SetDescription(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Description text cannot be null or empty.", nameof(text));
    }

    public void UploadImage(string path)
    {
    }

    public void SelectGreencityPicture(int index)
    {
        if (index <= 0)
            throw new ArgumentException("Index must be greater than zero.", nameof(index));
        // click on the gallery button and select a picture by index
    }

    public void SetDate(string date)
    {
        if (string.IsNullOrWhiteSpace(date))
            throw new ArgumentException("Date cannot be null or empty.", nameof(date));
    }

    public void SetTime(string start, string end)
    {
        if (string.IsNullOrWhiteSpace(start) || string.IsNullOrWhiteSpace(end))
            throw new ArgumentException("Start and End times must be provided.");
    }

    public void SelectOnlineLocation(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("URL must be provided for online location.", nameof(url));
    }

    public void ClickPublish()
    {
    }

    public void ClickPreview()
    {
    }

    public void ClickCancel()
    {
    }
}