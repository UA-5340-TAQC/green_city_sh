using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class NewsRichTextEditorComponent : RichTextEditorComponent
{
    // Основні частини редактора
    private By Toolbar => By.CssSelector(".ql-toolbar.ql-snow"); //панель інструментів редактора
    private By EditorContainer => By.CssSelector(".ql-container.ql-snow"); //контейнер редактора
    private By EditorArea => By.CssSelector(".ql-editor"); //область введення тексту

    // Кнопки форматування
    private By BoldButton => By.CssSelector("button.ql-bold"); //кнопка Bold
    private By ItalicButton => By.CssSelector("button.ql-italic"); //кнопка Italic
    private By UnderlineButton => By.CssSelector("button.ql-underline"); //кнопка Underline
    private By OrderedListButton => By.CssSelector("button.ql-list[value='ordered']"); //нумерований список
    private By BulletListButton => By.CssSelector("button.ql-list[value='bullet']"); //маркований список

    public NewsRichTextEditorComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public NewsRichTextEditorComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }

    public void SetText(string text)
    {
        //Ввести текст у редактор
    }

    public string GetText()
    {
        //Отримати текст із редактора
        return "";
    }

    public void ApplyFormatting(string formatType)
    {
        //Застосувати форматування за типом
    }

    public void ClickBold()
    {
        //Клікнути Bold
    }

    public void ClickItalic()
    {
        //Клікнути Italic
    }

    public void ClickUnderline()
    {
        //Клікнути Underline
    }

    public void ClickOrderedList()
    {
        //Клікнути Ordered List
    }

    public void ClickBulletList()
    {
        //Клікнути Bullet List
    }
}