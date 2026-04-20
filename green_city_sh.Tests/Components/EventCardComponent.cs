using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class EventCardComponent : BaseComponent
{
    // --- Locators ---
    // Header Section
    private By RatingStarsLocator => By.CssSelector(".stars-filled img");
    private By BookmarkButtonLocator => By.CssSelector(".favourite-button");
    private By EventImageLocator => By.CssSelector("img.event-image");

    // Metadata & Info
    private By TagsLocator => By.CssSelector(".tag-active");
    private By DateLocator => By.CssSelector(".date-container .date");
    private By TimeLocator => By.CssSelector(".time");
    private By LocationLocator => By.CssSelector(".date-container p");
    private By StatusLabelLocator => By.CssSelector(".event-status");

    // Content
    private By TitleLocator => By.CssSelector(".event-title");

    // Action Buttons
    private By MoreButtonLocator => By.CssSelector(".btn-group .secondary-global-button");
    private By JoinButtonLocator => By.CssSelector(".event-button");

    // Footer Stats
    private By PublicationDateLocator => By.CssSelector(".date p");
    private By AuthorNameLocator => By.CssSelector(".author p");
    private By CommentsCountLocator => By.CssSelector(".frame p");
    private By LikeCounterLocator => By.CssSelector(".like span");
    private By DislikeCounterLocator => By.CssSelector(".dislike span");

    // --- Constructors ---
    public EventCardComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public EventCardComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }

    // --- Essential Methods ---
    #region Data Extraction

    public string GetTitle() => RootElement.FindElement(TitleLocator).Text.Trim();

    public string GetDateAndTime()
    {
        string date = RootElement.FindElement(DateLocator).Text.Trim();
        string time = RootElement.FindElement(TimeLocator).Text.Trim();
        return $"{date} {time}".Trim(); 
    }

    public string GetLocation() => RootElement.FindElement(LocationLocator).Text.Trim();

    public string GetStatus() => RootElement.FindElement(StatusLabelLocator).Text.Trim();

    public int GetRating() => RootElement.FindElements(RatingStarsLocator).Count;

    #endregion

    #region Interactions

    public void ClickMore() => RootElement.FindElement(MoreButtonLocator).Click();

    public void ClickBookmark() => RootElement.FindElement(BookmarkButtonLocator).Click();

    public void ClickJoinEvent()
    {
        var joinButtons = RootElement.FindElements(JoinButtonLocator);

        if (joinButtons.Count > 0)
        {
            joinButtons[0].Click();
        }
        else
        {
            // The button is completely missing from the HTML
            throw new NoSuchElementException("The Join Event button is not present on this card.");
        }
    }

    #endregion

    #region State Verification

    public bool IsClosed() => 
        GetStatus().Equals("Closed", StringComparison.OrdinalIgnoreCase);

    public bool IsJoined()
    {
        try
        {
            var joinBtn = RootElement.FindElement(JoinButtonLocator);
            return !joinBtn.Enabled;
        }
        catch (NoSuchElementException)
        {
            // If the join button doesn't exist on the card, we assume the user isn't joined
            return false;
        }
    }

    #endregion

}