using OpenQA.Selenium;
using green_city_sh.Tests.Infrastructure;

namespace green_city_sh.Tests.Components;

public class ImageUploadComponent : BaseComponent
{
    private By ImageBrowseLink => By.XPath(".//span[normalize-space()='browse']");
    private By ImageUploadInput => By.XPath(".//input[@type='file']");
    private By DropZone => By.XPath(".//*[contains(@class, 'dropzone')]"); 
    
    public ImageUploadComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public ImageUploadComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }
    public void Upload(string imagePath)
    {
        var fullPath = Path.GetFullPath(imagePath);
        FindElement(ImageUploadInput).SendKeys(fullPath);
    }

    public String GetImageBrowseLinkText() =>
        FindElement(ImageBrowseLink).Text;

    public bool IsDropZoneDisplayed =>
        FindElement(DropZone).Displayed; 
}
