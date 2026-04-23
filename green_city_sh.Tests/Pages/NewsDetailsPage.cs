using green_city_sh.Tests.Components;
using green_city_sh.Tests.Modals;
using OpenQA.Selenium;

namespace green_city_sh.Tests.Pages;

public class NewsDetailsPage : BasePage
{
    public NewsDetailsPage(IWebDriver driver) : base(driver)
    {
    }

    private CommentComponent Comment => 
        new CommentComponent(driver, By.XPath("//app-comments-list/div"));
    private DeleteCommentModal DeleteCommentModal => 
        new DeleteCommentModal(driver, By.XPath("//app-warning-pop-up"));
    private CommentInputComponent CommentInput => 
        new CommentInputComponent(driver, By.XPath("//app-add-comment"));
    private CommentSectionComponent CommentSection => 
        new CommentSectionComponent(driver, By.XPath("//app-comments-container"));
    public void OpenNewsDetailsPage(int newsId)
    {
        var currentUrl = driver.Url;
        var uri = new Uri(currentUrl);
        driver.Navigate().GoToUrl($"{uri.Scheme}://{uri.Host}/#/greenCity/events/{newsId}");
    }

    public NewsDetailsPage AddComment(string text)
    {
        CommentInput.EnterComment(text);
        CommentInput.ClickSubmitCommentBtn();

        wait.Until(_ => GetComments().Any(c => c.DoesTextContain(text)));
        return this;
    }

    public NewsDetailsPage DeleteComment()
    {
        var firstComment = GetComments().First();
        firstComment.ClickDeleteCommentBtn();
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
        WaitUntilPageLoads();
        return this;
    }

    public IList<CommentComponent> GetComments() =>
        CommentSection.GetComments();

    public IList<CommentComponent> GetReplies() =>
        CommentSection.GetReplyComments();

    public int GetCounterNumber() =>
        CommentSection.GetNumberCounter();


}