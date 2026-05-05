using Allure.Net.Commons.Attributes;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace green_city_sh.Tests.Components;

public class CommentComponent : BaseComponent
{
    private By ReplyCommentField =>
        By.XPath(".//div[contains(@class, 'comment-body-wrapper')]//div[@class='comment-textarea']");

    private By EditCommentField => By.XPath(".//div[@class='comment-textarea']");
    private By UploadImgBtn => By.XPath(".//button[contains(@class, 'image-upload-btn')]");
    private By EmojiBtn => By.XPath(".//*[contains(@class, 'emoji-picker-btn')]");
    private By SubmitReplyBtn => By.XPath(".//button[contains(@class, 'primary-global-button__reply')]");
    private By AuthorName => By.XPath(".//span[@class='author-name']");
    private By DeleteCommentBtn => By.XPath(".//button[contains(@class, 'delete')]");
    private By EditCommentBtn => By.XPath(".//button[contains(@class, 'edit')]");
    private By ReplyCommentBtn => By.XPath(".//button[contains(@class, 'reply')]");
    private By DateComment => By.XPath(".//*[contains(@class, 'comment-date-month')]");
    private By CommentText => By.XPath(".//*[@class='comment-text']");

    private By ViewRepliesBtn =>
        By.XPath(".//button[.//span[contains(text(), 'View') or contains(text(), 'Переглянути')]]");

    private By HideRepliesBtn =>
        By.XPath(".//button[.//span[contains(text(), 'Hide') or contains(text(), 'Сховати')]]");

    private By ReplyCommentText =>
        By.XPath(".//div[contains(@class, 'wrapper-reply ng-star-inserted')]//div[@class='comment-text']");

    private By CancelEditBtn => By.XPath(".//button[contains(@class, 'cancel-edit')]");
    private By SaveEditBtn => By.XPath(".//button[contains(@class, 'save-edit')]");
    private By RepliesLocator => By.XPath(".//div[contains(@class, 'wrapper-reply')]");

    private By EditedLabel => By.XPath(".//span[contains(@class, 'edited')]");
    private By SubmitCommentBtn => By.XPath(".//button[@class='primary-global-button']");
    private By CommentField => By.XPath(".//div[@class='comment-textarea']");
    private By FileInput => By.XPath(".//input[@type='file']");
    private By ImagePreview => By.XPath(".//img[@class='image-preview']");

    private By SubmitCommentButton =>
        By.XPath(".//button[contains(text(), 'Коментар') or contains(text(), 'Comment')]");

    public CommentComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public CommentComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }

    [AllureStep("Enter reply comment text: {0}")]
    public void EnterReplyComment(string text)
    {
        wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));

        wait.Until(d =>
        {
            var field = d.FindElement(ReplyCommentField);
            field.Clear();
            field.SendKeys(text);
            return true;
        });
    }

    [AllureStep("Enter edited comment text: {0}")]
    public void EnterEditComment(string text) =>
        WaitAndTypeText(EditCommentField, text);

    [AllureStep("Click 'submit reply' button")]
    public void ClickSubmitReplyBtn() =>
        WaitAndClick(SubmitReplyBtn);

    [AllureStep("Enter comment text: {0}")]
    public void EnterComment(string text) =>
        WaitAndTypeText(CommentField, text);

    [AllureStep("Click 'submit' comment button")]
    public void ClickSubmitCommentBtn() =>
        WaitAndClick(SubmitCommentButton);

    [AllureStep("Click 'delete' comment button")]
    public void ClickDeleteCommentBtn()
    {
        var deleteBtn = wait.Until(d =>
        {
            var elements = RootElement.FindElements(DeleteCommentBtn);
            return elements.Count > 0 ? elements[0] : null;
        });
        deleteBtn.Click();
    }

    [AllureStep("Click 'view reply' button")]
    public void ClickViewRepliesBtn() =>
            WaitAndClick(ViewRepliesBtn);

    [AllureStep("Click 'hide reply' button")]
    public void ClickHideRepliesBtn() =>
        WaitAndClick(HideRepliesBtn);

    [AllureStep("Click 'edit' comment button")]
    public void ClickEditCommentBtn() =>
        WaitAndClick(EditCommentBtn);

    [AllureStep("Click 'reply' comment button")]
    public void ClickReplyCommentBtn() =>
        WaitAndClick(ReplyCommentBtn);

    [AllureStep("Click 'upload image' button")]
    public void ClickUploadImgBtn() =>
        WaitAndClick(UploadImgBtn);

    [AllureStep("Click 'emoji' button")]
    public void ClickEmojiBtn() =>
        WaitAndClick(EmojiBtn);

    [AllureStep("Click 'cancel edit' button")]
    public void ClickCancelEditBtn() =>
        WaitAndClick(CancelEditBtn);

    [AllureStep("Click 'save edit' button")]
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

    public bool IsCommentFieldEmpty() =>
        string.IsNullOrWhiteSpace(RootElement.FindElement(CommentField).GetAttribute("value"));

    [AllureStep("Upload image in comment section: {0}")]
    public void UploadImage(string fileName)
    {
        string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string filePath = Path.Combine(currentDirectory, "TestData", fileName);

        IWebElement fileInputElement = RootElement.FindElement(FileInput);
        fileInputElement.SendKeys(filePath);
    }

    public bool IsCommentButtonDisabled()
    {
        var btn = RootElement.FindElement(SubmitCommentBtn);

        return !btn.Enabled || btn.GetAttribute("disabled") != null;
    }

    public bool IsImagePreviewDisplayed()
    {
        return wait.Until(driver =>
        {
            var img = RootElement.FindElement(ImagePreview);
            return img.Displayed;
        });
    }

    public string? GetLastComment() =>
        RootElement.FindElements(CommentText).FirstOrDefault()?.Text;

    public string? GetLastReplyComment()
    {
        WaitUntilElementVisibleBy(ReplyCommentText);
        return RootElement.FindElements(ReplyCommentText).FirstOrDefault()?.Text;
    }

    public bool IsEditedLabelDisplayed()
    {
        WaitUntilElementVisibleBy(EditedLabel);
        return RootElement.FindElement(EditedLabel).Displayed;
    }

    public string GetReplyButtonAttribute() =>
        RootElement.FindElement(ReplyCommentBtn).GetAttribute("class") ?? string.Empty;

    public bool IsViewBtnDisplayed() =>
        FindElements(ViewRepliesBtn).Any(e => e.Displayed);

    public bool IsHideBtnDisplayed() =>
        FindElements(HideRepliesBtn).Any(e => e.Displayed);
}

