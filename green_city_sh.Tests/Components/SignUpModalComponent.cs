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

    private static readonly By InvalidEmailMessageLocator =
        By.CssSelector("app-error[controlname='email'] .margining");

    private static readonly By PasswordMismatchMessageLocator =
        By.XPath(".//div[contains(text(),'Passwords do not match')]");

    private static readonly By UserAlreadyExistsMessageLocator =
        By.XPath(".//input[contains(@class,'wrong-input')]");

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
                    {
                        return modal;
                    }
                }

                return null;
            })
            ?? throw new WebDriverTimeoutException("Sign-up modal did not appear.");

        return new SignUpModalComponent(driver, modalRoot);
    }

    public void FillRegistrationForm(string email, string userName, string password)
    {
        EnterEmail(email);
        EnterUserName(userName);
        EnterPassword(password);
        EnterConfirmPassword(password);
    }

    public void EnterEmail(string email)
    {
        WaitAndTypeText(EmailInputLocator, email);
    }

    public void EnterUserName(string userName)
    {
        WaitAndTypeText(UserNameInputLocator, userName);
    }

    public void EnterPassword(string password)
    {
        WaitAndTypeText(PasswordInputLocator, password);
    }

    public void EnterConfirmPassword(string password)
    {
        WaitAndTypeText(ConfirmPasswordInputLocator, password);
    }

    public void ClickSignUp()
    {
        WaitAndClick(SignUpButtonLocator);
    }

    public bool IsSignUpButtonEnabled()
    {
        var button = RootElement.FindElement(SignUpButtonLocator);
        return button.Enabled;
    }

    public string GetSuccessMessage()
    {
        string? message = wait.Until(driver =>
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
        {
            throw new WebDriverTimeoutException("Success message was not displayed.");
        }

        return message;
    }

    private bool IsAnyElementDisplayed(By locator)
    {
        try
        {
            return wait.Until(_ =>
                RootElement.FindElements(locator)
                    .Any(e => e.Displayed));
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }

    public bool IsInvalidEmailMessageDisplayed()
    {
        return IsAnyElementDisplayed(InvalidEmailMessageLocator);
    }

    public bool IsPasswordMismatchMessageDisplayed()
    {
        return IsAnyElementDisplayed(PasswordMismatchMessageLocator);
    }

    public bool IsUserAlreadyExistsErrorDisplayed()
    {
        return IsAnyElementDisplayed(UserAlreadyExistsMessageLocator);
    }

    public bool IsModalVisible()
    {
        try
        {
            return wait.Until(_ =>
                driver.FindElements(RootLocator)
                    .Any(e => e.Displayed));
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }
}
