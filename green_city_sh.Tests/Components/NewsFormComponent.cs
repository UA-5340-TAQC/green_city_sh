using OpenQA.Selenium;
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
    private By InvalidTitleTextarea => By.CssSelector("textarea[formcontrolname='title'].ng-invalid");
    private By TouchedInvalidTitleTextarea => By.CssSelector("textarea[formcontrolname='title'].ng-invalid.ng-touched");

    // ===== Source =====
    private By SourceBlock => By.CssSelector(".source-block"); 
    private By SourceWrapper => By.CssSelector(".source-block .title-wrapper"); //обгортка заголовка поля та підказки
    private By SourceInput => By.CssSelector("input[formcontrolname='source']"); //поле для посилання на зовнішнє джерело
    private By SourceFieldInfo => By.CssSelector(".source-block .field-info"); //текст-підказка для поля Source

    // ===== Buttons =====
    private By SubmitButtonsBlock => By.CssSelector(".submit-buttons"); //контейнер кнопок форми
    private By CancelButton => By.CssSelector(".submit-buttons .tertiary-global-button"); //кнопка Cancel
    private By PreviewButton => By.CssSelector(".submit-buttons .secondary-global-button"); //кнопка Preview
    private By PublishButton => By.CssSelector(".submit-buttons .primary-global-button[type='submit']"); //кнопка Publish

    // ===== Metadata =====
    private By MetadataBlock => By.CssSelector(".date"); //блок з датою та автором
    private By DateLabel => By.XPath(".//p[.//span[normalize-space()='Date:']]/span[2]"); //значення дати
    private By AuthorLabel => By.XPath(".//p[.//span[normalize-space()='Author:']]/span[2]"); //значення автора

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

    public void EnterTitle(string title)
    {
        
    }

    public void ClickTitleField()
    {
        
    }

    public void FocusAndBlurTitleField()
    {
        
    }

    public void SelectTags(params string[] tags)
    {
        
        Tags.SelectTags(tags);
    }

    public void EnterSource(string url)
    {
        
    }

    public void ClickSourceField()
    {
        
    }

    public void EnterContent(string text)
    {
               RichTextEditor.SetText(text);
    }

    public void UploadImage(string filePath)
    {
        
        ImageUpload.Upload(filePath);
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

    public bool IsTitleFieldInvalid()
    {
        
        return false;
    }

    public bool IsTitleFieldTouchedAndInvalid()
    {
        
        return false;
    }

    public bool IsSourceFieldValid()
    {
              return false;
    }

    public bool IsSourceFieldInvalid()
    {
              return false;
    }

    public bool IsPublishButtonEnabled()
    {
                return false;
    }

    public string GetTitleCharacterCount()
    {
              return "";
    }

    public string GetSourceFieldInfoText()
    {
                return "";
    }

    public string GetDate()
    {
                return "";
    }

    public string GetAuthor()
    {
                return "";
    }
}