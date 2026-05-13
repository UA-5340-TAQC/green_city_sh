using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using Allure.Net.Commons.Attributes;
using green_city_sh.Tests.Infrastructure;

namespace green_city_sh.Tests.Components;

public class NewsFormComponent : BaseComponent
{
    public NewsTagsComponent Tags { get; private set; }
    public NewsRichTextEditorComponent RichTextEditor { get; private set; }
    public NewsImageUploadComponent ImageUpload { get; private set; }

    // ===== Title =====
    private By TitleBlock => By.CssSelector(".title-block");
    private By TitleWrapper => By.CssSelector(".title-wrapper");
    private By NewsTitleTextarea => By.CssSelector("textarea[formcontrolname='title']");
    private By TitleCounter => By.CssSelector(".title-block .field-info");

    // ===== Source =====
    private By SourceBlock => By.CssSelector(".source-block");
    private By SourceWrapper => By.CssSelector(".source-block .title-wrapper");
    private By SourceInput => By.CssSelector("input[formcontrolname='source']");
    private By SourceFieldInfo => By.CssSelector(".source-block .field-info");

    // ===== Buttons =====
    private By SubmitButtonsBlock => By.CssSelector(".submit-buttons");
    private By CancelButton => By.CssSelector(".submit-buttons .tertiary-global-button");
    private By PreviewButton => By.CssSelector(".submit-buttons .secondary-global-button");
    private By PublishButton => By.CssSelector(".submit-buttons .primary-global-button[type='submit']");

    // ===== Metadata =====
    private By MetadataBlock => By.CssSelector(".date");
    private By DateLabel => By.XPath(".//p[.//span[normalize-space()='Date:']]/span[2]");
    private By AuthorLabel => By.XPath(".//p[.//span[normalize-space()='Author:']]/span[2]");

    public NewsFormComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
        Tags = new NewsTagsComponent(driver, RootElement.FindElement(By.CssSelector("div[formarrayname='tags']")));
        RichTextEditor = new NewsRichTextEditorComponent(driver, RootElement.FindElement(By.CssSelector("quill-editor[formcontrolname='content']")));
        ImageUpload = new NewsImageUploadComponent(driver, RootElement.FindElement(By.CssSelector(".image-block")));
    }

    public NewsFormComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
        Tags = new NewsTagsComponent(driver, RootElement.FindElement(By.CssSelector("div[formarrayname='tags']")));
        RichTextEditor = new NewsRichTextEditorComponent(driver, RootElement.FindElement(By.CssSelector("quill-editor[formcontrolname='content']")));
        ImageUpload = new NewsImageUploadComponent(driver, RootElement.FindElement(By.CssSelector(".image-block")));
    }

    [AllureStep("Enter Title: '{title}'")]
    public void EnterTitle(string title)
    {
        var element = wait.Until(ExpectedConditions.ElementToBeClickable(NewsTitleTextarea));
        element.Clear();
        element.SendKeys(title);
    }

    [AllureStep("Clear and Blur Title Field")]
    public void ClearAndBlurTitleField()
    {
        var element = wait.Until(ExpectedConditions.ElementToBeClickable(NewsTitleTextarea));
        element.Click();
        element.SendKeys(Keys.Control + "a");
        element.SendKeys(Keys.Backspace);
        element.SendKeys(Keys.Tab);
    }

    [AllureStep("Click Title Field")]
    public void ClickTitleField()
    {
        wait.Until(ExpectedConditions.ElementToBeClickable(NewsTitleTextarea)).Click();
    }

    [AllureStep("Focus and Blur Title Field")]
    public void FocusAndBlurTitleField()
    {
        var element = wait.Until(ExpectedConditions.ElementToBeClickable(NewsTitleTextarea));
        element.Click();
        element.SendKeys(Keys.Tab);
    }
    [AllureStep("Select Tags")]
    public void SelectTags(params string[] tags)
    {
        Tags.SelectTags(tags);
    }

    [AllureStep("Enter Source: '{url}'")]
    public void EnterSource(string url)
    {
        var element = wait.Until(ExpectedConditions.ElementIsVisible(SourceInput));
        element.Clear();
        element.SendKeys(url);
    }

    [AllureStep("Click Source Field")]
    public void ClickSourceField()
    {
        wait.Until(ExpectedConditions.ElementToBeClickable(SourceInput)).Click();
    }
    [AllureStep("Clear and Blur Source Field")]
    public void ClearAndBlurSourceField()
    {
        var element = wait.Until(ExpectedConditions.ElementToBeClickable(SourceInput));
        element.Click();
        element.SendKeys(Keys.Control + "a");
        element.SendKeys(Keys.Backspace);
        element.SendKeys(Keys.Tab);
    }
    [AllureStep("Enter Content")]
    public void EnterContent(string text)
    {
        RichTextEditor.SetText(text);
    }

    [AllureStep("Upload Image: '{fileName}'")]
    public void UploadImage(string fileName)
    {
        ImageUpload.UploadImage(fileName);
    }
    [AllureStep("Click Publish")]
    public void ClickPublish()
    {
        wait.Until(ExpectedConditions.ElementToBeClickable(PublishButton)).Click();
    }

    [AllureStep("Click Preview")]
    public void ClickPreview()
    {
        wait.Until(ExpectedConditions.ElementToBeClickable(PreviewButton)).Click();
    }

    [AllureStep("Click Cancel")]
    public void ClickCancel()
    {
        wait.Until(ExpectedConditions.ElementToBeClickable(CancelButton)).Click();
    }

    [AllureStep("Check if Title Field is Invalid")]
    public bool IsTitleFieldInvalid()
    {
        var element = wait.Until(ExpectedConditions.ElementExists(NewsTitleTextarea));
        return element.GetAttribute("class")?.Contains("ng-invalid") ?? false;
    }

    [AllureStep("Check if Title Field is Touched and Invalid")]
    public bool IsTitleFieldTouchedAndInvalid()
    {
        var element = wait.Until(ExpectedConditions.ElementExists(NewsTitleTextarea));
        var classAttr = element.GetAttribute("class");

        return (classAttr?.Contains("ng-invalid") ?? false) && (classAttr?.Contains("ng-touched") ?? false);
    }

    [AllureStep("Check if Source Field is Valid")]
    public bool IsSourceFieldValid()
    {
        var element = wait.Until(ExpectedConditions.ElementExists(SourceInput));
        return element.GetAttribute("class")?.Contains("ng-valid") ?? false;
    }
    [AllureStep("Check if Source Field is Invalid")]
    public bool IsSourceFieldInvalid()
    {
        var element = wait.Until(ExpectedConditions.ElementExists(SourceInput));
        return element.GetAttribute("class")?.Contains("ng-invalid") ?? false;
    }
    [AllureStep("Check if Publish Button is Enabled")]
    public bool IsPublishButtonEnabled()
    {
        var btn = wait.Until(ExpectedConditions.ElementExists(PublishButton));
        return btn.Enabled && btn.GetAttribute("disabled") == null;
    }

    [AllureStep("Get Title Character Count")]
    public string GetTitleCharacterCount()
    {
        return wait.Until(ExpectedConditions.ElementIsVisible(TitleCounter)).Text.Trim();
    }

    [AllureStep("Get Source Field Info Text")]
    public string GetSourceFieldInfoText()
    {
        return wait.Until(ExpectedConditions.ElementIsVisible(SourceFieldInfo)).Text.Trim();
    }

    [AllureStep("Get Date")]
    public string GetDate()
    {
        return wait.Until(ExpectedConditions.ElementIsVisible(DateLabel)).Text.Trim();
    }

    [AllureStep("Get Author")]
    public string GetAuthor()
    {
        return wait.Until(ExpectedConditions.ElementIsVisible(AuthorLabel)).Text.Trim();
    }

    [AllureStep("Get Title Value")]
    public string GetTitleValue()
    {
        return wait.Until(ExpectedConditions.ElementIsVisible(NewsTitleTextarea)).GetAttribute("value") ?? string.Empty;
    }
    [AllureStep("Wait For URL To Contain: '{substring}'")]
    public void WaitForUrlToContain(string substring)
    {
        wait.Until(ExpectedConditions.UrlContains(substring));
    }

    [AllureStep("Check if Image Preview is Displayed")]
    public bool IsImagePreviewDisplayed()
    {
        return ImageUpload.IsImagePreviewDisplayed();
    }

    [AllureStep("Get Image Upload Warning Message")]
    public string GetImageUploadWarningMessage()
    {
        return ImageUpload.GetWarningMessage();
    }
}
