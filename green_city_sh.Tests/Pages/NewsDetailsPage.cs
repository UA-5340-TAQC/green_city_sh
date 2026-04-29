using System.Text.RegularExpressions;
using green_city_sh.Tests.Components;
using green_city_sh.Tests.Modals;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace green_city_sh.Tests.Pages;

public class NewsDetailsPage : BasePage
{
    private CommentComponent? comment;
    private DeleteCommentModal? deleteComment;
    private CommentInputComponent? commentInput;
    private NewsInfoComponent? newsInfo;


    private By CommentsLocator => By.XPath("//div[contains(@class, 'wrapper-comment')]");
    private By RepliesLocator => By.XPath("//div[contains(@class, 'wrapper-reply')]");
    private By ViewRepliesBtn => By.XPath("//button[.//span[contains(text(), 'View') or contains(text(), 'Переглянути')]]");
    private By HideRepliesBtn => By.XPath("//button[.//span[contains(text(), 'Hide') or contains(text(), 'Сховати')]]");
    private By CommentCounter => By.Id("total-count");
    public NewsDetailsPage(IWebDriver driver) : base(driver)
    {
    }

    private CommentComponent Comment => comment ??= new CommentComponent(driver, By.XPath("//app-comments-list/div"));

    private DeleteCommentModal DeleteCommentModal => deleteComment ??= new DeleteCommentModal(driver, By.XPath("//app-warning-pop-up"));
    private CommentInputComponent CommentInput => commentInput ??= new CommentInputComponent(driver, By.XPath("//app-add-comment"));
    private NewsInfoComponent NewsInfo => newsInfo ??= new NewsInfoComponent(driver, By.XPath("//*[@class='news-info']"));
    public void OpenNewsDetailsPage(int newsId)
    {
        var currentUrl = driver.Url;
        var uri = new Uri(currentUrl);
        driver.Navigate().GoToUrl($"{uri.Scheme}://{uri.Host}/#/greenCity/news/{newsId}");
    }

    public NewsDetailsPage AddComment(string text)
    {
        CommentInput.EnterComment(text);
        CommentInput.ClickSubmitCommentBtn();

        wait.Until(_ => GetComments().Any(c => c.DoesTextContain(text)));
        return this;
    }

    public NewsDetailsPage EditComment(string text)
    {
        Comment.ClickEditCommentBtn();
        Comment.EnterEditComment(text);
        Comment.ClickSaveEditBtn();
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

    public NewsDetailsPage ReplyComment(string text)
    {
        Comment.ClickReplyCommentBtn();
        Comment.EnterReplyComment(text);
        Comment.ClickSubmitReplyBtn();
        return this;
    }


    public NewsDetailsPage ClickYesDelete()
    {
        DeleteCommentModal.ClickYesDeleteBtn();
        WaitUntilPageLoads();
        return this;
    }

    public NewsDetailsPage ClickViewReplies()
    {
        Comment.ClickViewRepliesBtn();
        return this;
    }

    public NewsDetailsPage ClickHideReplies()
    {
        Comment.ClickHideRepliesBtn();
        return this;
    }

    public IList<CommentComponent> GetComments()
    {
        IList<IWebElement> commentsList = driver.FindElements(CommentsLocator);

        IList<CommentComponent> comments = new List<CommentComponent>();
        foreach (var comment in commentsList)
        {
            comments.Add(new CommentComponent(driver, comment));
        }

        return comments;
    }

    public IList<CommentComponent> GetReplies()
    {
        var allReplies = new List<CommentComponent>();
        var clickedButtons = new HashSet<IWebElement>();
        var hasClicked = false;

        while (!hasClicked)
        {
            var replyButtons = driver.FindElements(ViewRepliesBtn);
            var unclickedButton = replyButtons.FirstOrDefault(btn => btn.Displayed && btn.Enabled && !clickedButtons.Contains(btn));
            if (unclickedButton == null)
                break;

            clickedButtons.Add(unclickedButton);
            unclickedButton.Click();

            var newReplies = driver.FindElements(RepliesLocator)
                .Where(r => r.Displayed)
                .Select(r => new CommentComponent(driver, r))
                .ToList();
            allReplies.AddRange(newReplies);
        }

        return allReplies;
    }

    public IList<CommentComponent> GetAllCommentsWithReplies()
    {
        var allComments = GetComments().ToList();
        if (driver.FindElements(RepliesLocator).Any())
        {
            var replies = GetReplies();
            allComments.AddRange(replies);
        }
        return allComments;
    }

    public int WaitForCommentCounterVisible()
    {
        wait.Until(ExpectedConditions.ElementIsVisible(CommentCounter));
        var value = 0;
        wait.Until(_ =>
        {
            try
            {
                var el = driver.FindElement(CommentCounter);
                var digits = new string(el.Text.Where(char.IsDigit).ToArray());
                return int.TryParse(digits, out value) && digits.Length > 0;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
        });
        return value;
    }

    public int WaitForCommentCounterToChange(int previousValue)
    {
        wait.Until(ExpectedConditions.ElementIsVisible(CommentCounter));
        var newValue = 0;
        wait.Until(_ =>
        {
            try
            {
                var digits = new string(driver.FindElement(CommentCounter).Text
                    .Where(char.IsDigit).ToArray());
                return int.TryParse(digits, out newValue) && newValue != previousValue;
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
        });
        return newValue;
    }

    public string? GetLastComment() =>
        Comment.GetLastComment();

    public string? GetLastReplyComment() =>
        Comment.GetLastReplyComment();
    public bool IsEditedLabelDisplayed() =>
        Comment.IsEditedLabelDisplayed();

    public string GetDateText() =>
        NewsInfo.GetDateText();

    public bool IsViewReplyBtnDisplayed() =>
        Comment.IsViewBtnDisplayed();

    public bool IsHideReplyBtnDisplayed() =>
        Comment.IsHideBtnDisplayed();

    public string GetAttributeReplyButton() =>
        Comment.GetReplyButtonAttribute;

}
