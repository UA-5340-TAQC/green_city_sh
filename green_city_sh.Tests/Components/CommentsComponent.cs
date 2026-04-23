﻿﻿using green_city_sh.Tests.Infrastructure;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace green_city_sh.Tests.Components;

public class CommentsComponent : BaseComponent
{
    public static readonly By RootLocator = By.CssSelector("app-add-comment");
    public CommentsComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public CommentsComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }

    public static CommentsComponent WaitAndCreate(IWebDriver driver)
    {
        new Actions(driver)
            .ScrollToElement(driver.FindElement(CommentItemLocator))
            .Perform();

        var root = new WebDriverWait(driver,
                           TimeSpan.FromSeconds(Configuration.DefaultTimeout))
                       .Until(drv =>
                       {
                           foreach (var el in drv.FindElements(RootLocator))
                               if (el.Displayed)
                                   return el;
                           return null;
                       })
                   ?? throw new WebDriverTimeoutException("Comments section did not become visible");
        
        return new CommentsComponent(driver, root);
    }

    public bool IsCommentFieldFocus()
    {
        var commentField = RootElement.FindElement(CommentInputLocator);
        var activeElement = driver.SwitchTo().ActiveElement();
        return commentField.Equals(activeElement);
    }
    
    private static readonly By CommentInputLocator = 
        By.CssSelector("div.comment-textarea[contenteditable='true']");
    
    private static readonly By SubmitButtonLocator =
        By.XPath(".//button[contains(normalize-space(.), 'Comment')]");
    
    private static readonly By CommentCountLocator = 
        By.CssSelector("span#total-count");
    
    private static readonly By CommentItemLocator = 
        By.CssSelector("div.comment-body-wrapper.wrapper-comment");
    
    private static readonly By CommentTextLocator =
        By.CssSelector("div.comment-text");

    private static readonly By CommentAuthorLocator =
        By.CssSelector("span.author-name");
    
    private static readonly By DeleteButtonLocator =
        By.XPath(".//button[contains(normalize-space(.), 'Delete')]");

    private static readonly By ConfirmDeleteButtonLocator =
        By.CssSelector("app-warning-pop-up .m-btn.primary-global-button");
    
    public CommentsComponent ClickCommentField()
    {
        RootElement.FindElement(CommentInputLocator).Click();
        return this;
    }

    public CommentsComponent EnterComment(string text)
    {
        var input = RootElement.FindElement(CommentInputLocator);
        input.Click();
        input.Clear();
        input.SendKeys(text);
        return this;
    }
    
    public string GetCommentInputText()
    => RootElement.FindElement(CommentInputLocator).Text;

    public void SubmitComment()
    {
        var submitButton = new WebDriverWait(driver,
            TimeSpan.FromSeconds(Configuration.DefaultTimeout))
            .Until(_ =>
                {
                    var btn = RootElement.FindElement(SubmitButtonLocator);
                    return btn.Enabled && btn.Displayed ? btn : null;
                });
            
        submitButton!.Click();
    }

    public int GetCommentCount()
    {
        var elements = driver.FindElements(CommentCountLocator);
        if (elements.Count == 0) return 0;
        
        new Actions(driver)
            .ScrollToElement(elements[0])
            .Perform();
        
        return int.TryParse(elements[0].Text.Trim(), out var count) ? count : 0;
    }

    public string GetFirtsCommentText()
    {
        var firstComment =  driver
            .FindElements(CommentItemLocator)
            .FirstOrDefault(e => e.Displayed)
            ?? throw new NoSuchElementException("No visible comments found");
        
        return firstComment.FindElement(CommentTextLocator).Text;
    }

    public string GetFirstCommentAuthor()
    {
        var firstComment = driver
                               .FindElements(CommentItemLocator)
                               .FirstOrDefault(e => e.Displayed)
                           ?? throw new NoSuchElementException("No visible comments found.");
        
        return firstComment.FindElement(CommentAuthorLocator).Text;
    }

    public bool IsCommentVisible(string text)
    {
        new Actions(driver)
            .ScrollToElement(driver.FindElement(CommentItemLocator))
            .Perform();

            return driver
                .FindElements(CommentItemLocator)
                .Any(c => c.Displayed && c.FindElements(CommentTextLocator)
                    .Any(t => t.Text.Contains(text)));

    }

    public void DeleteComment(string text)
    {
        var comment = driver
            .FindElements(CommentItemLocator)
            .FirstOrDefault(c => c.Displayed && c.FindElements(CommentTextLocator)
                .Any(t => string.Equals(t.Text.Trim(), text, StringComparison.Ordinal)))
        ?? throw new NoSuchElementException(
            $"Comment with '{text}' was not found");
        
        new Actions(driver)
            .ScrollToElement(comment)
            .Perform();
        
        comment.FindElement(DeleteButtonLocator).Click();
        
        new WebDriverWait(driver,
                TimeSpan.FromSeconds(Configuration.DefaultTimeout))
            .Until(drv =>
                drv.FindElements(By.CssSelector("app-warning-pop-up"))
                    .Any(e => e.Displayed));
        
        driver.FindElement(ConfirmDeleteButtonLocator).Click();
        
        new WebDriverWait(driver,
            TimeSpan.FromSeconds(Configuration.DefaultTimeout))
            .Until(_ => !IsCommentVisible(text));
    }
}
