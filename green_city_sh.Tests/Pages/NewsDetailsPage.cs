using System;
using System.Collections.Generic;
using System.Linq;
using Allure.Net.Commons.Attributes;
using green_city_sh.Tests.Components;
using green_city_sh.Tests.Modals;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace green_city_sh.Tests.Pages;

public class NewsDetailsPage : BasePage
{
    private CommentComponent? comment;
    private DeleteCommentModal? deleteComment;
    private CommentComponent? commentInput;
    private NewsInfoComponent? newsInfo;

    private By CommentsLocator => By.XPath("//div[contains(@class, 'wrapper-comment')]");
    private By RepliesLocator => By.XPath("//div[contains(@class, 'wrapper-reply')]");
    private By ViewRepliesBtn => By.XPath("//button[.//span[contains(text(), 'View') or contains(text(), 'Переглянути')]]");
    private By HideRepliesBtn => By.XPath("//button[.//span[contains(text(), 'Hide') or contains(text(), 'Сховати')]]");
    private By CommentCounter => By.Id("total-count");

    // --- News Details & Deletion Locators ---
    private By NewsTitle => By.CssSelector(".news-title-container .news-title");
    private By DeleteNewsButton => By.CssSelector(".edit-delete-block .delete-news-button");
    private By DeleteConfirmYesButton => By.CssSelector("app-dialog-pop-up .primary-global-button");
    private By SourceLinkLocator => By.CssSelector(".source-text");

    public NewsDetailsPage(IWebDriver driver) : base(driver)
    {
    }

    private CommentComponent Comment => comment ??= new CommentComponent(driver, By.XPath("//app-comments-list"));
    private DeleteCommentModal DeleteCommentModal => deleteComment ??= new DeleteCommentModal(driver, By.XPath("//app-warning-pop-up"));
    private CommentComponent CommentInput => commentInput ??= new CommentComponent(driver, By.XPath("//app-add-comment"));
    private NewsInfoComponent NewsInfo => newsInfo ??= new NewsInfoComponent(driver, By.XPath("//*[@class='news-info']"));

    [AllureStep("Open News Details Page by ID: {newsId}")]
    public void OpenNewsDetailsPage(int newsId)
    {
        var currentUrl = driver.Url;
        var uri = new Uri(currentUrl);
        driver.Navigate().GoToUrl($"{uri.Scheme}://{uri.Host}/#/greenCity/news/{newsId}");
    }

    [AllureStep("Add comment with text: {text}")]
    public NewsDetailsPage AddComment(string text)
    {
        CommentInput.EnterComment(text);
        CommentInput.ClickSubmitCommentBtn();

        wait.Until(_ => GetComments().Any(c => c.DoesTextContain(text)));
        return this;
    }

    [AllureStep("Edit comment with text: {text}")]
    public NewsDetailsPage EditComment(string text)
    {
        Comment.ClickEditCommentBtn();
        Comment.EnterEditComment(text);
        Comment.ClickSaveEditBtn();
        return this;
    }

    [AllureStep("Delete Comment")]
    public NewsDetailsPage DeleteComment(string text)
    {
        Comment.ClickDeleteCommentBtn(text);
        return this;
    }

    [AllureStep("Click Cancel Delete")]
    public NewsDetailsPage ClickCancelDelete()
    {
        DeleteCommentModal.ClickCancelDeleteBtn();
        return this;
    }

    [AllureStep("Reply to Comment: '{text}'")]
    public NewsDetailsPage ReplyComment(string text)
    {
        Comment.ClickReplyCommentBtn();
        Comment.EnterReplyComment(text);
        Comment.ClickSubmitReplyBtn();
        return this;
    }

    [AllureStep("Click Yes to Delete Comment")]
    public NewsDetailsPage ClickYesDelete()
    {
        DeleteCommentModal.ClickYesDeleteBtn();
        WaitUntilPageLoads();
        return this;
    }

    [AllureStep("Click View Replies")]
    public NewsDetailsPage ClickViewReplies()
    {
        Comment.ClickViewRepliesBtn();
        return this;
    }

    [AllureStep("Click Hide Replies")]
    public NewsDetailsPage ClickHideReplies()
    {
        Comment.ClickHideRepliesBtn();
        return this;
    }

    [AllureStep("Get Comments")]
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

    [AllureStep("Get Replies")]
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

    [AllureStep("Get All Comments With Replies")]
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

    [AllureStep("Wait for comment counter visible")]
    public int WaitForCommentCounterVisible()
    {
        var value = 0;
        var previousValue = -1;

        wait.Until(_ =>
        {
            try
            {
                var el = driver.FindElement(CommentCounter);
                if (!el.Displayed) return false;
                var digits = new string(el.Text.Where(char.IsDigit).ToArray());
                if (!int.TryParse(digits, out var current) || digits.Length == 0)
                    return false;

                if (current == previousValue)
                {
                    value = current;
                    return true;
                }

                previousValue = current;
                return false;
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

    [AllureStep("Wait for comment counter to change from: {previousValue}")]
    public int WaitForCommentCounterToChange(int previousValue)
    {
        var newValue = 0;
        var lastSeen = -1;
        wait.Until(_ =>
        {
            try
            {
                var el = driver.FindElement(CommentCounter);
                if (!el.Displayed)
                    return false;
                var digits = new string(el.Text.Where(char.IsDigit).ToArray());
                if (!int.TryParse(digits, out var current) || digits.Length == 0)
                    return false;
                if (current == lastSeen && current != previousValue)
                {
                    newValue = current;
                    return true;
                }

                lastSeen = current;
                return false;
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
        return newValue;
    }

    [AllureStep("Get Last Comment")]
    public string? GetLastComment() => Comment.GetLastComment();

    [AllureStep("Get Last Reply Comment")]
    public string? GetLastReplyComment() => Comment.GetLastReplyComment();

    [AllureStep("Check if Edited Label Displayed")]
    public bool IsEditedLabelDisplayed() => Comment.IsEditedLabelDisplayed(); [AllureStep("Get Date Text")]
    public string GetDateText() => NewsInfo.GetDateText(); [AllureStep("Check if View Reply Button Displayed")]
    public bool IsViewReplyBtnDisplayed() => Comment.IsViewBtnDisplayed();

    [AllureStep("Check if Hide Reply Button Displayed")]
    public bool IsHideReplyBtnDisplayed() => Comment.IsHideBtnDisplayed();

    [AllureStep("Get Reply Button Attribute")]
    public string GetAttributeReplyButton() => Comment.GetReplyButtonAttribute();

    // --- News Postcondition & Extraction Methods ---

    [AllureStep("Get News Title Text")]
    public string GetNewsTitleText()
    {
        return wait.Until(ExpectedConditions.ElementIsVisible(NewsTitle)).Text.Trim();
    }

    [AllureStep("Check if Source Link is Displayed")]
    public bool IsSourceLinkDisplayed()
    {
        return driver.FindElements(SourceLinkLocator).Count > 0;
    }

    [AllureStep("Click Delete News Button")]
    public NewsDetailsPage ClickDeleteNewsButton()
    {
        wait.Until(ExpectedConditions.ElementToBeClickable(DeleteNewsButton)).Click();
        return this;
    }

    [AllureStep("Confirm Deletion")]
    public void ConfirmDeletion()
    {
        wait.Until(ExpectedConditions.ElementToBeClickable(DeleteConfirmYesButton)).Click();
        wait.Until(ExpectedConditions.UrlContains("/news"));
    }
}
