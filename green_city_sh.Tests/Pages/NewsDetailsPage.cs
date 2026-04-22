using green_city_sh.Tests.Components;
using green_city_sh.Tests.Modals;
using OpenQA.Selenium;

namespace green_city_sh.Tests.Pages;

public class NewsDetailsPage : BasePage
{

    private By CommentsLocator => By.XPath("//app-comments-list[@datatype='comment']/div");
    public NewsDetailsPage(IWebDriver driver) : base(driver)
    {
    }

    private CommentComponent Comment => 
        new CommentComponent(driver, By.XPath("//app-comments-container"));
    private DeleteCommentModal DeleteCommentModal => 
        new DeleteCommentModal(driver, By.XPath("//app-warning-pop-up"));
    public void OpenNewsDetailsPage(int newsId)
    {
        var currentUrl = driver.Url;
        var uri = new Uri(currentUrl);
        driver.Navigate().GoToUrl($"{uri.Scheme}://{uri.Host}/#/greenCity/events/{newsId}");
    }

    public NewsDetailsPage AddComment(string text)
    {
        Comment.EnterComment(text);
        Comment.ClickSubmitCommentBtn();
        return this;
    }

    public NewsDetailsPage DeleteComment()
    {
        Comment.ClickDeleteCommentBtn();
        return this;
    }

    public NewsDetailsPage ClickCancelDelete()
    {
        DeleteCommentModal.ClickCancelDeleteBtn();
        return this;
    }

    public NewsDetailsPage ClickYesDelete()
    {
        DeleteCommentModal.ClickYesDeleteBtn();
        return this;
    }
    public int GetCommentsCount() =>
        Comment.GetNumberCounter();
    public IList<CommentComponent> GetComments() {
        IList<IWebElement> commentsList = driver.FindElements(CommentsLocator);

        IList<CommentComponent> comments = new List<CommentComponent>();
        foreach (var comment in commentsList)
        {
            comments.Add(new CommentComponent(driver, comment));
        }

        return comments;
    }
}