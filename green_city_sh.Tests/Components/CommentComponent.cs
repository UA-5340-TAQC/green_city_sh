using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class CommentComponent : BaseComponent
{
    private By ReplyCommentField => By.XPath(".//div[contains(@class, 'comment-body-wrapper')]//div[@class='comment-textarea']"); 
    private By UploadImgBtn => By.XPath(".//button[contains(@class, 'image-upload-btn')]");
    private By EmojiBtn => By.XPath(".//*[contains(@class, 'emoji-picker-btn')]");
    private By SubmitCommentBtn => By.XPath(".//button[@class='primary-global-button']");
    private By AuthorName => By.XPath(".//span[@class='author-name']");
    private By DeleteCommentBtn => By.XPath(".//button[contains(@class, 'delete')]");
    private By EditCommentBtn => By.XPath(".//button[contains(@class, 'edit')]");   
    private By ReplyCommentBtn => By.XPath(".//button[contains(@class, 'reply')]");
    private By DateComment => By.XPath(".//*[contains(@class, 'comment-date-month')]");
    private By CommentText => By.XPath(".//*[@class='comment-text']");
    private By CancelEditBtn => By.XPath(".//button[contains(@class, 'cancel-edit')]");
    private By SaveEditBtn => By.XPath(".//button[contains(@class, 'save-edit')]");
    private By ViewRepliesBtn => By.XPath(".//button[.//span[contains(text(), 'View') or contains(text(), 'Переглянути')]]");
    private By HideRepliesBtn => By.XPath(".//button[.//span[contains(text(), 'Hide') or contains(text(), 'Сховати')]]");
    
    public CommentComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public CommentComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }
    
    public void EnterReplyComment(string text) =>
        WaitAndTypeText(ReplyCommentField, text);
    public void ClickSubmitCommentBtn() =>
        WaitAndClick(SubmitCommentBtn);
    public void ClickDeleteCommentBtn() =>
        WaitAndClick(DeleteCommentBtn);
    public void ClickEditCommentBtn() =>
        WaitAndClick(EditCommentBtn);
    public void ClickReplyCommentBtn() =>
        WaitAndClick(ReplyCommentBtn);
    public void ClickUploadImgBtn() =>
        WaitAndClick(UploadImgBtn);
    public void ClickEmojiBtn() => 
        WaitAndClick(EmojiBtn);
    public void ClickCancelEditBtn() =>
        WaitAndClick(CancelEditBtn);
    public void ClickSaveEditBtn() => 
        WaitAndClick(SaveEditBtn);
    public string GetDate() => 
        RootElement.FindElement(DateComment).Text;

    public string GetTextComment() =>
        RootElement.FindElement(CommentText).Text;

    public string GetAuthorName() =>
        RootElement.FindElement(AuthorName).Text;
    
    public bool DoesTextContain(string text) =>
        GetTextComment().Contains(text, StringComparison.OrdinalIgnoreCase);
    
}
