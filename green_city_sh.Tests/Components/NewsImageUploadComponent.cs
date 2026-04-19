using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class NewsImageUploadComponent : ImageUploadComponent
{
    // Основні частини блоку завантаження. Іще є такий ньанс: до завантаження зображення відображається одна структура
    // (зона drag-and-drop), після завантаження - інша (cropper). Тому локатори розділені на дві групи.
    private By DropZone => By.CssSelector(".dropzone"); //зона drag-and-drop для завантаження зображення
    private By TextWrapper => By.CssSelector(".text-wrapper"); //текстовий блок усередині drop zone
    private By BrowseLabel => By.CssSelector("label[for='upload']"); //посилання/label browse
    private By FileUploadInput => By.CssSelector("input[type='file']#upload"); //input для завантаження файлу

    // Cropper
    private By CropperBlock => By.CssSelector(".cropper-block"); //блок cropper після завантаження зображення
    private By CropperImage => By.CssSelector("img.ngx-ic-source-image"); //зображення в cropper
    private By CropperArea => By.CssSelector(".ngx-ic-cropper"); //область cropper

    // Кнопки
    private By CancelButton => By.XPath(".//button[normalize-space()='Cancel']"); //кнопка Cancel у блоці cropper
    private By SubmitButton => By.XPath(".//button[normalize-space()='Submit']"); //кнопка Submit у блоці cropper

    // Повідомлення
    private By WarningMessage => By.CssSelector(".warning"); //попередження про допустимий формат і розмір файлу

    public NewsImageUploadComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public NewsImageUploadComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }

    public void Upload(string filePath)
    {
        //Завантажити файл
    }

    public bool IsDropZoneDisplayed()
    {
        //Перевірити, чи відображається зона завантаження
        return false;
    }

    public bool IsBrowseDisplayed()
    {
        //Перевірити, чи відображається browse
        return false;
    }

    public bool IsCropperDisplayed()
    {
        //Перевірити, чи відображається cropper після завантаження зображення
        return false;
    }

    public bool IsImagePreviewDisplayed()
    {
        //Перевірити, чи відображається прев'ю зображення
        return false;
    }

    public void ClickCancel()
    {
        //Клікнути Cancel у блоці cropper
    }

    public void ClickSubmit()
    {
        //Клікнути Submit у блоці cropper
    }

    public string GetWarningMessage()
    {
        //Отримати текст warning message
        return "";
    }
}