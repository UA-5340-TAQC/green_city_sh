using OpenQA.Selenium;

namespace green_city_sh.Tests.Pages;

public class CreateUpdateEventPage : BasePage
{
    // Left column (General Info)
    private By TitleField => By.CssSelector("input[formcontrolname='title']"); // title entry field
    private By DurationDropdown => By.CssSelector("mat-select[formcontrolname='duration']"); // selector for event length

    // Initiative type buttons
    private By InitiativeTypeButton(string type) => By.XPath($"//button[contains(text(), '{type}')]");

    private By EventTypeDropdown => By.CssSelector("mat-select[formcontrolname='isOpen']"); // dropdown Open/Closed
    private By InviteDropdown => By.CssSelector("mat-select[formcontrolname='invite']"); // dropdown Invite

    private By DescriptionEditor => By.CssSelector("textarea[formcontrolname='description'], .ql-editor"); // rich text description field

    // Upload images
    private By AddPictureButton => By.CssSelector("input[type='file']"); // + button for adding images
    private By UploadedImagePreview => By.CssSelector(".image-preview"); // preview of uploaded image
    private By ClosePictureIcon => By.CssSelector(".close-icon"); // x icon to remove uploaded image
    private By GreencityPictureItem(int index) => By.CssSelector($".images-container img:nth-child({index}), .gallery-item:nth-child({index})"); // a specific picture by index

    // Right Column (Schedule & Location)
    private By DatePickerInput => By.CssSelector("input[matinput][formcontrolname='date']"); // date picker input field
    private By StartTimeInput => By.CssSelector("input[formcontrolname='startTime']"); // start time input field
    private By EndTimeInput => By.CssSelector("input[formcontrolname='endTime']"); // end time input field
    private By AllDayCheckbox => By.CssSelector("mat-checkbox[formcontrolname='allDay']"); // checkbox All day
    private By PlaceLocationCheckbox => By.XPath("//mat-checkbox[.//span[contains(text(), 'Place')]]"); // checkbox for physical location
    private By OnlineLocationCheckbox => By.XPath("//mat-checkbox[.//span[contains(text(), 'Online')]]"); // checkbox for online location
    private By OnlineLinkField => By.CssSelector("input[formcontrolname='onlineLink']"); // input field for online event link

    // Bottom Controls
    private By CancelLink => By.CssSelector(".cancel-btn"); // Cancel button
    private By PreviewButton => By.CssSelector(".preview-btn"); // Preview button
    private By PublishButton => By.CssSelector(".submit-btn, .publish-btn"); // Publish button

    public CreateUpdateEventPage(IWebDriver driver) : base(driver)
    {
    }

    // Essential methods
    public void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be null or empty.", nameof(title));

        // set the name of the Title field
    }

    public void SelectInitiativeTypes(params string[] types)
    {
        if (types == null || types.Length == 0)
            throw new ArgumentException("At least one initiative type must be provided.", nameof(types));

        // click on the buttons of the initiative types
    }

    public void SetDescription(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Description text cannot be null or empty.", nameof(text));

        // set the description
    }

    public void UploadImage(string path)
    {
        // upload a file
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

        // set the date
    }

    public void SetTime(string start, string end)
    {
        if (string.IsNullOrWhiteSpace(start) || string.IsNullOrWhiteSpace(end))
            throw new ArgumentException("Start and End times must be provided.");

        // set the start and end time
    }

    public void SelectOnlineLocation(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("URL must be provided for online location.", nameof(url));

        // click the online location checkbox and set the URL
    }

    public void ClickPublish()
    {
        // click the publish button
    }

    public void ClickPreview()
    {
        // click the preview button
    }

    public void ClickCancel()
    {
        // click the cancel button
    }
}