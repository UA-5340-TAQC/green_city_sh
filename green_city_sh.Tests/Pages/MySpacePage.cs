using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;

namespace green_city_sh.Tests.Pages;

public class MySpacePage : BasePage
{
    private readonly By _userName = By.CssSelector("app-profile-header .name");
    private readonly By _userLocation = By.CssSelector("app-profile-header .location");
    private readonly By _userRate = By.XPath("//div[contains(@class, 'rate')]/p");
    private readonly By _editProfileIcon = By.CssSelector("app-profile-header .edit-icon");
    
    private readonly By _acquiredHabitsStat = By.XPath("//div[@class='chain']//p[contains(text(), 'acquired habits')]/preceding-sibling::p");
    private readonly By _habitsInProgressStat = By.XPath("//div[@class='chain']//p[contains(text(), 'habits in progress')]/preceding-sibling::p");
    private readonly By _publishedNewsStat = By.XPath("//div[@class='chain']//p[contains(text(), 'published news')]/preceding-sibling::p");
    private readonly By _organizedEventsStat = By.XPath("//div[@class='chain']//p[contains(text(), 'organized and attended events')]/preceding-sibling::p");

    private readonly By _myHabitsTab = By.XPath("//span[contains(@class, 'mdc-tab__text-label') and contains(text(), 'My habits')]");
    private readonly By _myNewsTab = By.XPath("//span[contains(@class, 'mdc-tab__text-label') and contains(text(), 'My news')]");
    private readonly By _myEventsTab = By.XPath("//span[contains(@class, 'mdc-tab__text-label') and contains(text(), 'My Events')]");

    private readonly By _myHabitsContainer = By.Id("mat-tab-content-0-0");
    private readonly By _myNewsContainer = By.Id("mat-tab-content-0-1");
    private readonly By _myEventsContainer = By.Id("mat-tab-content-0-2");

    private readonly By _addNewHabitButton = By.Id("create-button-new-habit");
    private readonly By _welcomeMessage = By.XPath("//*[contains(text(), 'Hi Eco Friend')]"); 

    private readonly By _notificationBadge = By.CssSelector(".notification-icon div");

    private readonly By _factOfTheDayDescription = By.CssSelector("app-profile-cards .card-description");
    private readonly By _seeAllAchievementsLink = By.CssSelector("app-users-achievements .title-achievements a");
    private readonly By _seeAllFriendsLink = By.CssSelector("app-users-friends a.text-more");
    
    public MySpacePage(IWebDriver driver) : base(driver)
    {
    }

    public MyHabitsTabComponent SwitchToMyHabits()
    {
        wait.Until(ExpectedConditions.ElementToBeClickable(_myHabitsTab)).ClickWithRetry();
        return new MyHabitsTabComponent(driver, _myHabitsContainer);
    }

    public MyNewsTabComponent SwitchToMyNews()
    {
        wait.Until(ExpectedConditions.ElementToBeClickable(_myNewsTab)).ClickWithRetry();
        return new MyNewsTabComponent(driver, _myNewsContainer);
    }

    public MyEventsTabComponent SwitchToMyEvents()
    {
        wait.Until(ExpectedConditions.ElementToBeClickable(_myEventsTab)).ClickWithRetry();
        return new MyEventsTabComponent(driver, _myEventsContainer);
    }

    public string GetUserName()
    {
        return wait.Until(ExpectedConditions.ElementIsVisible(_userName)).Text.Trim();
    }

    public string GetUserLocation()
    {
        return wait.Until(ExpectedConditions.ElementIsVisible(_userLocation)).Text.Trim();
    }

    public string GetUserRate()
    {
        return wait.Until(ExpectedConditions.ElementIsVisible(_userRate)).Text.Trim();
    }

    public int GetUserNotificationCount()
    {
        if (driver.IsElementVisible(_notificationBadge))
        {
            string countStr = driver.FindElement(_notificationBadge).Text.Trim();
            return int.TryParse(countStr, out int count) ? count : 0;
        }
        return 0;
    }

    public ProfileEditPage ClickEditProfile()
    {
        wait.Until(ExpectedConditions.ElementToBeClickable(_editProfileIcon)).ClickWithRetry();
        return new ProfileEditPage(driver);
    }

    public CreateHabitPage ClickAddNewHabit()
    {
        wait.Until(ExpectedConditions.ElementToBeClickable(_addNewHabitButton)).ClickWithRetry();
        return new CreateHabitPage(driver);
    }

    public string GetWelcomeMessageText()
    {
        if (driver.IsElementVisible(_welcomeMessage))
        {
            return driver.FindElement(_welcomeMessage).Text.Trim();
        }
        return string.Empty; 
    }
    
    public string GetFactOfTheDayText()
    {
        return wait.Until(ExpectedConditions.ElementIsVisible(_factOfTheDayDescription)).Text.Trim();
    }

    public void ClickSeeAllFriends()
    {
        wait.Until(ExpectedConditions.ElementToBeClickable(_seeAllFriendsLink)).ClickWithRetry();
    }

    public void ClickSeeAllAchievements()
    {
        wait.Until(ExpectedConditions.ElementToBeClickable(_seeAllAchievementsLink)).ClickWithRetry();
    }

    public int GetAcquiredHabitsCount() => int.Parse(driver.FindElement(_acquiredHabitsStat).Text);
    public int GetHabitsInProgressCount() => int.Parse(driver.FindElement(_habitsInProgressStat).Text);
    public int GetPublishedNewsCount() => int.Parse(driver.FindElement(_publishedNewsStat).Text);
    public int GetOrganizedEventsCount() => int.Parse(driver.FindElement(_organizedEventsStat).Text);
}