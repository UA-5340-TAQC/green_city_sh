using OpenQA.Selenium;
using green_city_sh.Tests.Infrastructure;

namespace green_city_sh.Tests.Components;

public class NewsFormComponent : BaseComponent
{
    public NewsTagsComponent Tags { get; private set; }
    public NewsRichTextEditorComponent RichTextEditor { get; private set; }
    public NewsImageUploadComponent ImageUpload { get; private set; }

    // ===== Title =====
    private By TitleBlock => By.CssSelector(".title-block"); //блок поля Title
    private By TitleWrapper => By.CssSelector(".title-wrapper"); //обгортка заголовка поля та лічильника
    private By NewsTitleTextarea => By.CssSelector("textarea[formcontrolname='title']"); //поле для введення заголовку
    private By TitleCounter => By.CssSelector(".title-block .field-info"); //лічильник символів заголовку
    private By InvalidTitleTextarea => By.CssSelector("textarea[formcontrolname='title'].ng-invalid"); //поле Title у невалідному стані
    private By TouchedInvalidTitleTextarea => By.CssSelector("textarea[formcontrolname='title'].ng-invalid.ng-touched"); //поле Title після втрати фокусу у невалідному стані

    // ===== Source =====
    private By SourceBlock => By.CssSelector(".source-block"); //блок поля Source
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
        //Ввести заголовок новини
    }

    public void ClickTitleField()
    {
        //Клікнути в поле Title
    }

    public void FocusAndBlurTitleField()
    {
        //Сфокусуватись на полі Title і прибрати фокус без введення тексту
    }

    public void SelectTags(params string[] tags)
    {
        //Обрати до 3 тегів
        Tags.SelectTags(tags);
    }

    public void EnterSource(string url)
    {
        //Ввести посилання на джерело
    }

    public void ClickSourceField()
    {
        //Клікнути в поле Source
    }

    public void EnterContent(string text)
    {
        //Ввести текст у редактор
        RichTextEditor.SetText(text);
    }

    public void UploadImage(string filePath)
    {
        //Завантажити зображення
        ImageUpload.Upload(filePath);
    }

    public void ClickPublish()
    {
        //Клікнути Publish
    }

    public void ClickPreview()
    {
        //Клікнути Preview
    }

    public void ClickCancel()
    {
        //Клікнути Cancel
    }

    public bool IsTitleFieldInvalid()
    {
        //Перевірити, чи поле Title у невалідному стані
        return false;
    }

    public bool IsTitleFieldTouchedAndInvalid()
    {
        //Перевірити, чи поле Title має стани touched та invalid
        return false;
    }

    public bool IsSourceFieldValid()
    {
        //Перевірити, чи поле Source у валідному стані
        return false;
    }

    public bool IsSourceFieldInvalid()
    {
        //Перевірити, чи поле Source у невалідному стані
        return false;
    }

    public bool IsPublishButtonEnabled()
    {
        //Перевірити, чи активна кнопка Publish
        return false;
    }

    public string GetTitleCharacterCount()
    {
        //Отримати лічильник символів заголовку
        return "";
    }

    public string GetSourceFieldInfoText()
    {
        //Отримати текст підказки для поля Source
        return "";
    }

    public string GetDate()
    {
        //Отримати дату створення/редагування новини
        return "";
    }

    public string GetAuthor()
    {
        //Отримати ім'я автора
        return "";
    }
}