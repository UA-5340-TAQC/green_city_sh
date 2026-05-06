using OpenQA.Selenium;
using Allure.NUnit.Attributes;

namespace green_city_sh.Tests.Components;

public class NewsListComponent : BaseComponent
{
    private By AllNewsCards => By.CssSelector("li.gallery-view-li-active");
    private By GridViewButton => By.CssSelector(".btn-tiles");
    private By ListViewButton => By.CssSelector(".btn-bars");

    public NewsListComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public NewsListComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }

    public int GetNewsCards()
    {
        return RootElement.FindElements(AllNewsCards).Count;
    }


    public List<IWebElement> GetNewsCardElements()
    {
        return RootElement.FindElements(AllNewsCards).ToList();
    }

    public void SwitchToGridView()
    {
        try
        {
            var gridBtn = RootElement.FindElement(GridViewButton);
            if (gridBtn.Displayed)
            {
                gridBtn.Click();
                Thread.Sleep(500);
            }
        }
        catch (NoSuchElementException)
        {
            // Grid button not found
        }
    }
    public void SwitchToListView()
    {
        try
        {
            var listBtn = RootElement.FindElement(ListViewButton);
            if (listBtn.Displayed)
            {
                listBtn.Click();
                Thread.Sleep(500);
            }
        }
        catch (NoSuchElementException)
        {
            // List button not found
        }
    }
    public void OpenNewsByIndex(int index)
    {
        var cards = GetNewsCardElements();
        if (index >= 0 && index < cards.Count)
        {
            cards[index].Click();
        }
    }

    public void ScrollToLoad()
    {

    }

    [AllureStep("Get all news cards as components")]
    /// <summary>
    /// Returns all news cards as a list of NewsCardComponent components.
    /// </summary>
    public List<NewsCardComponent> GetAllNewsCardsAsComponents()
    {
        var cardElements = GetNewsCardElements();
        var cards = new List<NewsCardComponent>();

        foreach (var cardElement in cardElements)
        {
            cards.Add(new NewsCardComponent(driver, cardElement));
        }

        return cards;
    }

    /// <summary>
    /// Waits while news cards are loading.
    /// </summary>
    [AllureStep("Wait for news cards to load")]
    public void WaitForCardsToLoad()
    {
        wait.Until(driver => GetNewsCards() > 0);
    }

    /// <summary>
    /// Waits for the news cards to be refreshed by checking if the number of cards changes from the specified previous count.
    /// </summary>
    /// <param name="previousCount">The number of cards before the refresh action</param>
    [AllureStep("Wait for news cards to refresh from {previousCount} cards")]
    public void WaitForCardsToRefresh(int previousCount)
    {
        wait.Until(driver => GetNewsCards() != previousCount);
    }

    /// <summary>
    /// Gets all news cards that contain a specific tag.
    /// </summary>
    public List<NewsCardComponent> GetNewsCardsWithTag(string tag)
    {
        var allCards = GetAllNewsCardsAsComponents();
        return allCards.Where(card => card.GetTags().Contains(tag.ToUpper())).ToList();
    }
}
