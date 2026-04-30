using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class CommentInputComponent : BaseComponent
{
    private By CommentField => By.XPath(".//div[@class='comment-textarea']");
    private By SubmitCommentButton => By.XPath(".//button[contains(text(), 'Коментар') or contains(text(), 'Comment')]");
    
    public CommentInputComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public CommentInputComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }
    public void EnterComment(string text) => 
        WaitAndTypeText(CommentField, text); 
    public void ClickSubmitCommentBtn() =>
        WaitAndClick(SubmitCommentButton);
}