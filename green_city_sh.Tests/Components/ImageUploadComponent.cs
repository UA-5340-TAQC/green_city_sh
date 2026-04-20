using OpenQA.Selenium;
using green_city_sh.Tests.Infrastructure;

namespace green_city_sh.Tests.Components;

public class ImageUploadComponent : BaseComponent
{
    private By ImageBrowseLink => By.XPath(".//span[normalize-space()='browse']");
    private By ImageUploadLink => By.XPath(".//input[@type='file']");
    private By DropZone => By.XPath(".//*[contains(@class, 'dropzone')]"); 
    
    protected ImageUploadComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    protected ImageUploadComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }
    public ImageUploadComponent UploadImage(string imagePath) {
        FindElement(ImageUploadLink).SendKeys(imagePath);
        return this;
    }

    public String GetImageBrowseLinkText() =>
        FindElement(ImageBrowseLink).Text;

    public bool IsDropZoneDisplayed =>
        FindElement(DropZone).Displayed; 
}
