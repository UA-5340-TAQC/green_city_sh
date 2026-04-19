using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class NewsTagsComponent : TagsComponent
{
    private By TagButtons => By.CssSelector("button.tag-button"); //усі кнопки тегів
    private By SelectedTags => By.CssSelector("a.global-tag.global-tag-clicked"); //усі обрані теги

    private By TagButtonByName(string tagName) =>
        By.XPath($".//button[.//span[normalize-space()='{tagName}']]"); //кнопка тега за назвою

    private By SelectedTagByName(string tagName) =>
        By.XPath($".//a[contains(@class,'global-tag-clicked')][.//span[normalize-space()='{tagName}']]"); //обраний тег за назвою

    public NewsTagsComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public NewsTagsComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }

    public void SelectTag(string name)
    {
        //Обрати тег за назвою
    }

    public void SelectTags(params string[] tags)
    {
        //Обрати декілька тегів (до 3)
    }

    public bool IsTagSelected(string name)
    {
        //Перевірити, чи тег обраний
        return false;
    }

    public int GetSelectedTagsCount()
    {
        //Повернути кількість обраних тегів
        return 0;
    }
}