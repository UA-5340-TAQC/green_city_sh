using Allure.Net.Commons.Attributes;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace green_city_sh.Tests.Components;

public class NewsRichTextEditorComponent : RichTextEditorComponent
{

    private By Toolbar => By.CssSelector(".ql-toolbar.ql-snow");
    private By EditorContainer => By.CssSelector(".ql-container.ql-snow");
    private By EditorArea => By.CssSelector(".ql-editor");


    private By BoldButton => By.CssSelector("button.ql-bold");
    private By ItalicButton => By.CssSelector("button.ql-italic");
    private By UnderlineButton => By.CssSelector("button.ql-underline");
    private By OrderedListButton => By.CssSelector("button.ql-list[value='ordered']");
    private By BulletListButton => By.CssSelector("button.ql-list[value='bullet']");

    public NewsRichTextEditorComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public NewsRichTextEditorComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }

    [AllureStep("Set content text: '{text}'")]
    public void SetText(string text)
    {
        var editor = wait.Until(ExpectedConditions.ElementToBeClickable(EditorArea));

        editor.Click();
        editor.SendKeys(Keys.Control + "a");
        editor.SendKeys(Keys.Backspace);

        editor.SendKeys(text);
    }

    public string GetText()
    {
        return "";
    }

    public void ApplyFormatting(string formatType)
    {

    }

    public void ClickBold()
    {

    }

    public void ClickItalic()
    {

    }

    public void ClickUnderline()
    {

    }

    public void ClickOrderedList()
    {

    }

    public void ClickBulletList()
    {

    }
}
