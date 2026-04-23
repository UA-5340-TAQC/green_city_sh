using green_city_sh.Tests.Infrastructure;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace green_city_sh.Tests.Components;

public class SignUpModalComponent : BaseComponent
{
    public static readonly By RootLocator = By.CssSelector("app-sign-up");

    private static readonly By EmailInputLocator = By.CssSelector("input[formcontrolname='email']");
    private static readonly By UserNameInputLocator = By.CssSelector("input[formcontrolname='firstName']");
    private static readonly By PasswordInputLocator = By.CssSelector("input[formcontrolname='password']");
    private static readonly By ConfirmPasswordInputLocator = By.CssSelector("input[formcontrolname='repeatPassword']");

    private static readonly By SignUpButtonLocator = By.CssSelector("button[type='submit']");

    private static readonly By SuccessMessageLocator = By.CssSelector(".mat-mdc-snack-bar-label");

    private static readonly By InvalidEmailMessageLocator = By.CssSelector("app-error[controlname='email'] .margining");
    private static readonly By PasswordMismatchMessageLocator = By.XPath(".//div[contains(text(),'Passwords do not match')]");
    private static readonly By UserAlreadyExistsMessageLocator = By.XPath(".//input[contains(@class,'wrong-input')]");

    public SignUpModalComponent(IWebDriver driver, By rootLocator)
        : base(driver, rootLocator)
    {
    }

    public SignUpModalComponent(IWebDriver driver, IWebElement rootElement)
        : base(driver, rootElement)
    {
    }

    
    public static SignUpModalComponent WaitAndCreate(IWebDriver driver)
    {
        var modalRoot = new WebDriverWait(driver, TimeSpan.FromSeconds(Configuration.DefaultTimeout))
            .Until(drv =>
            {
                foreach (var modal in drv.FindElements(RootLocator))
                {
                    if (modal.Displayed)
                        return modal;
                }
                return null;
            })
            ?? throw new WebDriverTimeoutException("Sign-up modal did not appear.");

        return new SignUpModalComponent(driver, modalRoot);
    }

    
    public void EnterEmail(string email)
    {
        var input = RootElement.FindElement(EmailInputLocator);
        input.Clear();
        input.SendKeys(email);
    }

    public void EnterUserName(string userName)
    {
        var input = RootElement.FindElement(UserNameInputLocator);
        input.Clear();
        input.SendKeys(userName);
    }

    public void EnterPassword(string password)
    {
        var input = RootElement.FindElement(PasswordInputLocator);
        input.Clear();
        input.SendKeys(password);
    }

    public void EnterConfirmPassword(string password)
    {
        var input = RootElement.FindElement(ConfirmPasswordInputLocator);
        input.Clear();
        input.SendKeys(password);
    }

    public void ClickSignUp()
    {
        var button = RootElement.FindElement(SignUpButtonLocator);
        button.Click();
    }

   
    public bool IsSignUpButtonEnabled()
    {
        var button = RootElement.FindElement(SignUpButtonLocator);
        return button.Enabled;
    }

    public string GetSuccessMessage()
    {
        var message = wait.Until(driver =>
        {
            try
            {
                var elements = driver.FindElements(SuccessMessageLocator);
                var visibleMessage = elements.FirstOrDefault(e => e.Displayed);

                return visibleMessage != null ? visibleMessage.Text.Trim() : null;
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
        });

        if (string.IsNullOrWhiteSpace(message))
            throw new WebDriverTimeoutException("Success message was not displayed.");

        return message;
    }

    public bool IsInvalidEmailMessageDisplayed()
    {
        return RootElement.FindElements(InvalidEmailMessageLocator).Any();
    }

    public bool IsPasswordMismatchMessageDisplayed()
    {
        return RootElement.FindElements(PasswordMismatchMessageLocator).Any();
    }

    public bool IsUserAlreadyExistsErrorDisplayed()
    {
        return RootElement.FindElements(UserAlreadyExistsMessageLocator).Any();
    }

    public bool IsModalVisible()
    {
        return RootElement.Displayed;
    }
}

