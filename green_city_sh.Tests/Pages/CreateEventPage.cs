using green_city_sh.Tests.Infrastructure;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace green_city_sh.Tests.Pages;

public class CreateEventPage : BasePage
{
    private const string PageUrl = "/events/create-update-event";

    private static readonly By TitleInput = By.CssSelector("input[formcontrolname='title']");
    private static readonly By TitleError = By.CssSelector("mat-error.mat-mdc-form-field-error");
    private static readonly By DescriptionInput = By.CssSelector(".ql-editor");
    private static readonly By DescriptionError = By.CssSelector(".quill-counter.quill-valid");
    private static readonly By DateInput = By.CssSelector("input[placeholder='Choose a date']");
    private static readonly By StartTimeInput = By.CssSelector("input[formcontrolname='startTime']");
    private static readonly By EndTimeInput = By.CssSelector("input[formcontrolname='finishTime']");
    private static readonly By InviteDropdown = By.XPath("//p[normalize-space()='Event type']/following::mat-select[2]");
    private static readonly By InviteOption = By.XPath("//mat-option//span[normalize-space()='All']");
    private static readonly By OnlineCheckboxLabel = By.XPath("//label[normalize-space()='Online']");
    private static readonly By OnlineCheckboxInput = By.CssSelector("input[type='checkbox']");
    private static readonly By OnlineLinkInput = By.CssSelector("input[formcontrolname='onlineLink']");
    private static readonly By PublishButton = By.CssSelector("button.submit-buttons");


    public CreateEventPage(IWebDriver driver) : base(driver)
    {
    }

    public void Open()
    {
        driver.Navigate().GoToUrl(Configuration.BaseUrl + PageUrl);
        new WebDriverWait(driver, TimeSpan.FromSeconds(Configuration.DefaultTimeout))
            .Until(drv =>
                drv.FindElements(TitleInput).Any(el => el.Displayed));
    }

    // [AllureStep("Enter title: '{0}'")]
    public CreateEventPage EnterTitle(string title)
    {
        var input = driver.FindElement(TitleInput);
        input.Clear();
        input.SendKeys(title);
        return this;
    }

    public CreateEventPage BlurTitleField()
    {
        driver.FindElement(By.CssSelector("body")).Click();
        return this;
    }

    // [AllureStep("Enter Description")]
    public CreateEventPage EnterDescription(string text)
    {
        var field = driver.FindElement(DescriptionInput);
        field.Clear();
        field.SendKeys(text);
        return this;
    }

    public CreateEventPage EnterDate(string date)
    {
        var input = driver.FindElement(DateInput);
        input.Clear();
        input.SendKeys(date);
        return this;
    }

    // [AllureStep("Enter Start Time")]
    public CreateEventPage EnterStartTimeInput(string time)
    {
        var input = driver.FindElement(StartTimeInput);
        input.Clear();
        input.SendKeys(time);
        input.SendKeys(Keys.Escape);
        return this;
    }

    // [AllureStep("Enter Finish Time")]
    public CreateEventPage EnterEndTimeInput(string time)
    {
        var input = driver.FindElement(EndTimeInput);
        input.Clear();
        input.SendKeys(time);

        input.SendKeys(Keys.Escape);
        return this;
    }

    // [AllureStep("Select invite option")]
    public CreateEventPage SelectInvite()
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Configuration.DefaultTimeout));

        wait.Until(drv => drv.FindElements(InviteDropdown).Any(e => e.Displayed));
        driver.FindElement(InviteDropdown).Click();
        wait.Until(drv => drv.FindElements(InviteOption).Any(e => e.Displayed));
        driver.FindElement(InviteOption).Click();

        return this;
    }

    // [AllureStep("Click online checkbox")]
    public CreateEventPage ClickOnlineCheckbox()
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Configuration.DefaultTimeout));
        wait.Until(drv => drv.FindElements(OnlineCheckboxLabel).Any());

        var label = wait.Until(d => d.FindElement(
            By.XPath("//label[normalize-space()='Online']")
        ));
        label.Click();

        return this;
    }

    // [AllureStep("Enter online link")]
    public CreateEventPage EnterOnlineLink(string url)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Configuration.DefaultTimeout));
        wait.Until(drv => drv.FindElements(OnlineLinkInput).Any(e => e.Displayed));

        var input = driver.FindElement(OnlineLinkInput);
        input.Clear();
        input.SendKeys(url);
        return this;
    }

    public bool IsPublishButtonEnabled() => driver.FindElement(PublishButton).Enabled;
    public bool IsTitleErrorVisible() => driver.FindElements(TitleError).Any(e => e.Displayed);
    public bool IsDescriptionErrorVisible() => driver.FindElements(DescriptionError).Any();


    public string GetTitleErrorText()
    {
        new WebDriverWait(driver, TimeSpan.FromSeconds(Configuration.DefaultTimeout))
            .Until(drv => drv.FindElements(TitleError).Any(e => e.Displayed));
        return driver.FindElement(TitleError).Text.Trim();
    }
}