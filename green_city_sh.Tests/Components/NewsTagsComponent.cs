using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

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
        var tagButton = RootElement.FindElement(TagButtonByName(name));
        tagButton.Click();
        Thread.Sleep(500);
    }

    public void SelectTags(params string[] tags)
    {
        foreach (var tag in tags)
        {
            SelectTag(tag);
        }
    }
    public void SelectTagWithRealClick(string name)
    {
        var allTagButtons = RootElement.FindElements(TagButtons);

        IWebElement targetButton = null;
        foreach (var button in allTagButtons)
        {
            var tagText = button.FindElement(By.CssSelector(".text")).Text.Trim();
            if (tagText.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                targetButton = button;
                break;
            }
        }

        if (targetButton == null)
            throw new NoSuchElementException($"Tag '{name}' not found");

        var tagLink = targetButton.FindElement(By.CssSelector("a.global-tag"));

        var actions = new Actions(driver);
        actions.MoveToElement(tagLink)
               .ClickAndHold()
               .Release()
               .Perform();

        Thread.Sleep(1000);
    }

    public bool IsTagSelected(string name)
    {
        var elements = RootElement.FindElements(SelectedTagByName(name));
        return elements.Count > 0;
    }

    public int GetSelectedTagsCount()
    {
        return RootElement.FindElements(SelectedTags).Count;
    }

    public void ClearAllFilters()
    {
        var selectedTags = RootElement.FindElements(SelectedTags);
        foreach (var tag in selectedTags)
        {
            tag.Click();
        }
    }

    /// <summary>
    /// Returns a list of the names of all available tags on the page.
    /// </summary>
    public List<string> GetAllAvailableTags()
    {
        var tagButtons = RootElement.FindElements(TagButtons);
        var tags = new List<string>();

        foreach (var button in tagButtons)
        {
            var tagName = button.FindElement(By.CssSelector(".text")).Text.Trim();
            tags.Add(tagName);
        }

        return tags;
    }
}
