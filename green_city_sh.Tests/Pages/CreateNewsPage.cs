using OpenQA.Selenium;
using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;

namespace green_city_sh.Tests.Pages;

public class CreateNewsPage : BasePage
{
    public NewsFormComponent NewsForm => new(driver, By.CssSelector("form"));//форма створення/редагування новини

    public CreateNewsPage(IWebDriver driver) : base(driver)
    {
    }

    public void OpenCreateNewsPage()
    {
        //відкрити сторінку створення новини
        Open("/news/create-news");
    }

    public void EnterTitle(string title)
    {
        //Ввести заголовок новини
        NewsForm.EnterTitle(title);
    }

    public void ClickTitleField()
    {
        //Клікнути в поле Title
        NewsForm.ClickTitleField();
    }

    public void FocusAndBlurTitleField()
    {
        //Сфокусуватись на полі Title і прибрати фокус без введення тексту
        NewsForm.FocusAndBlurTitleField();
    }

    public void SelectTags(params string[] tags)
    {
        //Обрати до 3 тегів
        NewsForm.SelectTags(tags);
    }

    public void EnterSource(string url)
    {
        //Ввести посилання на джерело
        NewsForm.EnterSource(url);
    }

    public void ClickSourceField()
    {
        //Клікнути в поле Source
        NewsForm.ClickSourceField();
    }

    public void EnterContent(string text)
    {
        //Ввести текст у редактор
        NewsForm.EnterContent(text);
    }

    public void UploadImage(string filePath)
    {
        //Завантажити зображення
        NewsForm.UploadImage(filePath);
    }

    public void ClickPublish()
    {
        //Клікнути Publish
        NewsForm.ClickPublish();
    }

    public void ClickPreview()
    {
        //Клікнути Preview
        NewsForm.ClickPreview();
    }

    public void ClickCancel()
    {
        //Клікнути Cancel
        NewsForm.ClickCancel();
    }

    public bool IsTitleFieldInvalid()
    {
        //Перевірити, чи поле Title у невалідному стані
        return NewsForm.IsTitleFieldInvalid();
    }

    public bool IsTitleFieldTouchedAndInvalid()
    {
        //Перевірити, чи поле Title має стани touched та invalid
        return NewsForm.IsTitleFieldTouchedAndInvalid();
    }

    public bool IsSourceFieldValid()
    {
        //Перевірити, чи поле Source у валідному стані
        return NewsForm.IsSourceFieldValid();
    }

    public bool IsSourceFieldInvalid()
    {
        //Перевірити, чи поле Source у невалідному стані
        return NewsForm.IsSourceFieldInvalid();
    }

    public bool IsPublishButtonEnabled()
    {
        //Перевірити, чи активна кнопка Publish
        return NewsForm.IsPublishButtonEnabled();
    }

    public string GetTitleCharacterCount()
    {
        //Отримати лічильник символів заголовку
        return NewsForm.GetTitleCharacterCount();
    }

    public string GetSourceFieldInfoText()
    {
        //Отримати текст підказки для поля Source
        return NewsForm.GetSourceFieldInfoText();
    }

    public string GetDate()
    {
        //Отримати дату створення/редагування новини
        return NewsForm.GetDate();
    }

    public string GetAuthor()
    {
        //Отримати ім'я автора
        return NewsForm.GetAuthor();
    }
}