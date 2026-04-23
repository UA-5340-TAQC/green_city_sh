using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class CommentSectionComponent : BaseComponent
{
    private By CommentsLocator => By.XPath(".//div[contains(@class, 'comment-body-wrapper')]");
    private By RepliesLocator => By.XPath(".//div[contains(@class, 'wrapper-reply')]");
    private By ViewRepliesBtn => By.XPath(".//button[.//span[contains(text(), 'View') or contains(text(), 'Переглянути')]]");
    private By HideRepliesBtn => By.XPath(".//button[.//span[contains(text(), 'Hide') or contains(text(), 'Сховати')]]");
    private By CommentCounter => By.Id("total-count"); 
    public CommentSectionComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public CommentSectionComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }
    
    public IList<CommentComponent> GetComments() {
        IList<IWebElement> commentsList = RootElement.FindElements(CommentsLocator);

        IList<CommentComponent> comments = new List<CommentComponent>();
        foreach (var comment in commentsList)
        {
            comments.Add(new CommentComponent(driver, comment));
        }

        return comments;
    }

    public IList<CommentComponent> GetReplyComments()
    {
        var replyButtons = RootElement.FindElements(ViewRepliesBtn);
        foreach (var btn in replyButtons)
        {
            if (btn.Displayed && btn.Enabled)
                btn.Click();
        }
        wait.Until(d =>
            RootElement.FindElements(RepliesLocator).Count > 0);
        var replyElements = RootElement.FindElements(RepliesLocator);
        
        IList<CommentComponent> replies = new List<CommentComponent>();
        
        foreach (var reply in replyElements)
        {
            replies.Add(new CommentComponent(driver, reply));
        }
        return replies;
    }
    
    public int GetNumberCounter()
    {
        WaitUntilElementVisibleBy(CommentCounter);
        wait.Until(_ =>
        {
            var text = FindElement(CommentCounter).Text;
            return text.Any(char.IsDigit);
        });
        var textValue = FindElement(CommentCounter).Text;
        return int.Parse(new string(textValue.Where(char.IsDigit).ToArray()));
    }
    
    
}