using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using green_city_sh.Tests.Infrastructure;

namespace green_city_sh.Tests.Components;

public class SignUpModalComponent : BaseComponent
{
    public static readonly By RootLocator = By.CssSelector("app-sign-up");

    private static readonly By EmailInput = By.CssSelector("input[formcontrolname='email']");
    private static readonly By UserNameInput = By.CssSelector("input[formcontrolname='firstName']");
    private static readonly By PasswordInput = By.CssSelector("input[formcontrolname='password']");
    private static readonly By ConfirmPasswordInput = By.CssSelector("input[formcontrolname='repeatPassword']");
    private static readonly By SignUpButton = By.CssSelector("button[type='submit']");
    private static readonly By SuccessMessage = By.CssSelector(".mat-mdc-snack-bar-label");
    private static readonly By InvalidEmailMessage = By.XPath("//div[contains(text(),'Please check that your e-mail')]");

    public SignUpModalComponent(IWebDriver driver, By rootLocator)
        : base(driver, rootLocator) { }

    public SignUpModalComponent(IWebDriver driver, IWebElement rootElement)
        : base(driver, rootElement) { }

    public static SignUpModalComponent WaitAndCreate(IWebDriver driver)
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Configuration.DefaultTimeout));

        var modal = wait.Until(drv =>
        {
            var elements = drv.FindElements(RootLocator);
            return elements.FirstOrDefault(e => e.Displayed);
        });

        return new SignUpModalComponent(driver, modal!);
    }

    public void EnterEmail(string email)
    {
        var el = RootElement.FindElement(EmailInput);
        el.Clear();
        el.SendKeys(email);
    }

    public void EnterUserName(string name)
    {
        var el = RootElement.FindElement(UserNameInput);
        el.Clear();
        el.SendKeys(name);
    }

    public void EnterPassword(string password)
    {
        var el = RootElement.FindElement(PasswordInput);
        el.Clear();
        el.SendKeys(password);
    }

    public void EnterConfirmPassword(string password)
    {
        var el = RootElement.FindElement(ConfirmPasswordInput);
        el.Clear();
        el.SendKeys(password);
    }

    public void ClickSignUp()
    {
        RootElement.FindElement(SignUpButton).Click();
    }

    public bool IsSignUpButtonEnabled()
    {
        return RootElement.FindElement(SignUpButton).Enabled;
    }

    public string GetSuccessMessage()
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Configuration.DefaultTimeout));

        var message = wait.Until(drv =>
        {
            var el = drv.FindElement(SuccessMessage);
            return el.Displayed ? el : null;
        });

        return message!.Text.Trim();
    }

    public bool IsInvalidEmailMessageDisplayed()
    {
        return RootElement.FindElements(InvalidEmailMessage).Any();
    }
}