using OpenQA.Selenium;
using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;

namespace green_city_sh.Tests.Pages;

public class CreateNewsPage : BasePage
{
    private NewsFormComponent? newsForm;

    public NewsFormComponent NewsForm => newsForm ??= new(driver, By.CssSelector("form"));//форма створення/редагування новини
    public CreateNewsPage(IWebDriver driver) : base(driver)
    {
    }

    public void OpenCreateNewsPage()
    {
        
        string currentUrl = driver.Url;
        Uri uri = new Uri(currentUrl);
        driver.Navigate().GoToUrl($"{uri.Scheme}://{uri.Host}/#/greenCity/news/create-news");
    }

    public void EnterTitle(string title)
    {
        
        NewsForm.EnterTitle(title);
    }

    public void ClickTitleField()
    {
        
        NewsForm.ClickTitleField();
    }

    public void FocusAndBlurTitleField()
    {
        
        NewsForm.FocusAndBlurTitleField();
    }

    public void SelectTags(params string[] tags)
    {
        
        NewsForm.SelectTags(tags);
    }

    public void EnterSource(string url)
    {
        
        NewsForm.EnterSource(url);
    }

    public void ClickSourceField()
    {
        
        NewsForm.ClickSourceField();
    }

    public void EnterContent(string text)
    {
        
        NewsForm.EnterContent(text);
    }

    public void UploadImage(string filePath)
    {
        
        NewsForm.UploadImage(filePath);
    }

    public void ClickPublish()
    {
        
        NewsForm.ClickPublish();
    }

    public void ClickPreview()
    {
        
        NewsForm.ClickPreview();
    }

    public void ClickCancel()
    {
                NewsForm.ClickCancel();
    }

    public bool IsTitleFieldInvalid()
    {
                return NewsForm.IsTitleFieldInvalid();
    }

    public bool IsTitleFieldTouchedAndInvalid()
    {
            return NewsForm.IsTitleFieldTouchedAndInvalid();
    }

    public bool IsSourceFieldValid()
    {
                return NewsForm.IsSourceFieldValid();
    }

    public bool IsSourceFieldInvalid()
    {
        
        return NewsForm.IsSourceFieldInvalid();
    }

    public bool IsPublishButtonEnabled()
    {
        
        return NewsForm.IsPublishButtonEnabled();
    }

    public string GetTitleCharacterCount()
    {
        
        return NewsForm.GetTitleCharacterCount();
    }

    public string GetSourceFieldInfoText()
    {
                return NewsForm.GetSourceFieldInfoText();
    }

    public string GetDate()
    {
                return NewsForm.GetDate();
    }

    public string GetAuthor()
    {
                return NewsForm.GetAuthor();
    }
}