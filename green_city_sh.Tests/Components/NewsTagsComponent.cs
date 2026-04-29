using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class NewsTagsComponent : TagsComponent
{
    private By TagButtons => By.CssSelector("button.tag-button");
    private By SelectedTags => By.CssSelector("a.global-tag.global-tag-clicked");

    private By TagButtonByName(string tagName) =>
        By.XPath($".//button[.//span[normalize-space()='{tagName}']]");

    private By SelectedTagByName(string tagName) =>
        By.XPath($".//a[contains(@class,'global-tag-clicked')][.//span[normalize-space()='{tagName}']]");

    public NewsTagsComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public NewsTagsComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }

    public void SelectTag(string name)
    {

    }

    public void SelectTags(params string[] tags)
    {

    }

    public bool IsTagSelected(string name)
    {
        return false;
    }

    public int GetSelectedTagsCount()
    {
        return 0;
    }
}