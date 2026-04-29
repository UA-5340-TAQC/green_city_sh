using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class NewsImageUploadComponent : ImageUploadComponent
{

    private By DropZone => By.CssSelector(".dropzone");
    private By TextWrapper => By.CssSelector(".text-wrapper");
    private By BrowseLabel => By.CssSelector("label[for='upload']");
    private By FileUploadInput => By.CssSelector("input[type='file']#upload");


    private By CropperBlock => By.CssSelector(".cropper-block");
    private By CropperImage => By.CssSelector("img.ngx-ic-source-image");
    private By CropperArea => By.CssSelector(".ngx-ic-cropper");


    private By CancelButton => By.XPath(".//button[normalize-space()='Cancel']");
    private By SubmitButton => By.XPath(".//button[normalize-space()='Submit']");


    private By WarningMessage => By.CssSelector(".warning");

    public NewsImageUploadComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public NewsImageUploadComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }

    public void Upload(string filePath)
    {

    }

    public bool IsDropZoneDisplayed()
    {
        return false;
    }

    public bool IsBrowseDisplayed()
    {
        return false;
    }

    public bool IsCropperDisplayed()
    {
        return false;
    }

    public bool IsImagePreviewDisplayed()
    {
        return false;
    }

    public void ClickCancel()
    {

    }

    public void ClickSubmit()
    {

    }

    public string GetWarningMessage()
    {

        var warningElements = RootElement.FindElements(WarningMessage);
        if (warningElements.Count == 0)
        {
            return "";
        }

        return warningElements[0].GetDomProperty("textContent")?.Trim() ?? "";
    }
}