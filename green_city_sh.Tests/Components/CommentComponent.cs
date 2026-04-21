using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class CommentComponent : BaseComponent
{
    private By CommentField => By.XPath(".//div[@class='comment-textarea']");
    private By ReplyCommentField => By.XPath(".//div[contains(@class, 'comment-body-wrapper')]//div[@class='comment-textarea']"); 
    private By UploadImgBtn => By.XPath(".//button[contains(@class, 'image-upload-btn')]");
    private By EmojiBtn => By.XPath(".//*[contains(@class, 'emoji-picker-btn')]");
    private By SubmitCommentBtn => By.XPath(".//button[@class='primary-global-button']");
    private By AuthorName => By.XPath(".//span[@class='author-name']");
    private By DeleteCommentBtn => By.XPath(".//button[contains(@class, 'delete')]");
    private By EditCommentBtn => By.XPath(".//button[contains(@class, 'edit')]");   
    private By ReplyCommentBtn => By.XPath(".//button[contains(@class, 'reply')]");
    private By DateComment => By.XPath(".//*[contains(@class, 'comment-date-month')]");
    private By CommentsItems => By.XPath(".//*[contains(@class, 'comment-body-wrapper')]");
    private By CommentText => By.XPath(".//*[@class='comment-text']");
    private By CommentCounter => By.Id("total-count"); 
    
    public CommentComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public CommentComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }

    public void EnterComment(string text) => 
        WaitAndTypeText(CommentField, text); 
    public void EnterReplyComment(string text) =>
        WaitAndTypeText(ReplyCommentField, text);
    public void ClickSubmitCommentBtn() =>
        WaitAndClick(SubmitCommentBtn);
    public void ClickDeleteCommentBtn() =>
        WaitAndClickFirst(DeleteCommentBtn);
    public void ClickEditCommentBtn() =>
        WaitAndClickFirst(EditCommentBtn);
    public void ClickReplyCommentBtn() =>
        WaitAndClickFirst(ReplyCommentBtn);
    public void ClickUploadImgBtn() =>
        WaitAndClick(UploadImgBtn);
    public void ClickEmojiBtn() => 
        WaitAndClick(EmojiBtn);

    private IList<IWebElement> GetAllComments()
    {
        wait.Until(_ => RootElement.FindElements(CommentsItems).Any());
        return RootElement.FindElements(CommentsItems);
    } 
    
    public CommentComponent GetFirstComment()
    {
        var element = GetAllComments().First();
        return new CommentComponent(driver, element);
    }

    public string? GetFirstDate() => 
        RootElement.FindElements(DateComment).FirstOrDefault()?.Text;

    public string? GetFirstTextComment() =>
        RootElement.FindElements(CommentText).FirstOrDefault()?.Text;
    
    public string? GetFirstAuthorName() =>
        RootElement.FindElements(AuthorName).FirstOrDefault()?.Text;
    
    public int GetCommentCount() => 
        GetAllComments().Count;

    public int GetNumberCounter()
    {
        WaitUntilElementVisibleBy(CommentCounter);
        var number = FindElement(CommentCounter).Text;
        return int.Parse(new string(number.Where(char.IsDigit).ToArray()));
    }

    public void DeleteAllComments()
    {
        var i = 0;
        while (FindElements(DeleteCommentBtn).Any() && i < 20)
        {
            WaitAndClickFirst(DeleteCommentBtn);
            i++;
        }
    }
    public bool IsCommentDisplayed(string commentText) => 
        RootElement.FindElements(CommentText).Any(e => e.Text.Trim().Contains(commentText, StringComparison.OrdinalIgnoreCase));
    
}
