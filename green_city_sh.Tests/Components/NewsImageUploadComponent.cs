using System;
using System.IO;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using Allure.Net.Commons.Attributes;
using green_city_sh.Tests.Infrastructure;

namespace green_city_sh.Tests.Components;

public class NewsImageUploadComponent : BaseComponent
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

    public bool IsDropZoneDisplayed()
    {
        return this.FindElement(DropZone).Displayed;
    }

    public bool IsBrowseDisplayed()
    {
        return this.FindElement(BrowseLabel).Displayed;
    }

    public bool IsCropperDisplayed()
    {
        return false;
    }

    [AllureStep("Upload image: '{fileName}'")]
    public void UploadImage(string fileName)
    {
        string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string filePath = Path.Combine(currentDirectory, "TestData", fileName);

        // ElementExists is required because file inputs are typically visually hidden (display:none)
        var fileInputElement = wait.Until(ExpectedConditions.ElementExists(FileUploadInput));
        fileInputElement.SendKeys(filePath);
    }

    [AllureStep("Check if Image Preview is Displayed")]
    public bool IsImagePreviewDisplayed()
    {
        try
        {
            return wait.Until(ExpectedConditions.ElementIsVisible(CropperImage)).Displayed;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }

    public void ClickCancel()
    {
    }

    [AllureStep("Click Submit to close cropper")]
    public void ClickSubmit()
    {
        wait.Until(ExpectedConditions.ElementToBeClickable(SubmitButton)).Click();
        wait.Until(ExpectedConditions.InvisibilityOfElementLocated(CropperBlock));
    }

    [AllureStep("Get Image Upload Warning Message")]
    public string GetWarningMessage()
    {
        try
        {
            var el = wait.Until(ExpectedConditions.ElementIsVisible(WarningMessage));
            return el.GetDomProperty("textContent")?.Trim() ?? "";
        }
        catch (WebDriverTimeoutException)
        {
            return "";
        }
    }
}
