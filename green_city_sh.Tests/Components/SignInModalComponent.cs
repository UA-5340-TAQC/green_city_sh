using Allure.NUnit.Attributes;
using green_city_sh.Tests.Infrastructure;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace green_city_sh.Tests.Components;

public class SignInModalComponent : BaseComponent
{
    /// <summary>CSS locator for the modal root element.</summary>
    public static readonly By RootLocator = By.CssSelector("app-auth-modal");

    private static readonly By TitleLocator = By.CssSelector("h1");
    private static readonly By EmailInputLocator = By.CssSelector("input[type='email']");
    private static readonly By PasswordInputLocator = By.CssSelector("input[type='password'], input[type='text']");
    private static readonly By PasswordTogglerLocator = By.CssSelector("img[src*='eye'], img[class*='eye']");
    private static readonly By SignInButtonLocator = By.CssSelector("button[type='submit']");
    private static readonly By GoogleButtonLocator = By.XPath(".//button[contains(., 'Google')]");
    private static readonly By CloseButtonLocator = By.CssSelector("a.close-modal-window");
    private static readonly By ForgotPasswordLocator = By.XPath(".//a[contains(text(), 'Forgot password')]");
    private static readonly By SignUpLinkLocator = By.XPath(".//a[contains(text(), 'Sign up')]");
    private static readonly By EmailErrorLocator = By.CssSelector("#email-err-msg div");
    private static readonly By ErrorMessageLocator = By.CssSelector(".alert-general-error");

    /// <summary>
    /// Initializes the component by locating the root element via CSS locator.
    /// </summary>
    public SignInModalComponent(IWebDriver driver, By rootLocator)
        : base(driver, rootLocator)
    {
    }

    /// <summary>
    /// Initializes the component from an already-resolved root element.
    /// </summary>
    public SignInModalComponent(IWebDriver driver, IWebElement rootElement)
        : base(driver, rootElement)
    {
    }

    /// <summary>
    /// Waits for the sign-in modal to become visible,
    /// then returns a component bound to that exact DOM element.
    /// </summary>
    [AllureStep("Wait for sign-in modal to appear and create component")]
    public static SignInModalComponent WaitAndCreate(IWebDriver driver)
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
            ?? throw new WebDriverTimeoutException("Sign-in modal did not become visible.");

        return new SignInModalComponent(driver, modalRoot);
    }

    /// <summary>
    /// Clears the email field and types the given email address.
    /// </summary>
    [AllureStep("Enter email: {email}")]
    public SignInModalComponent EnterEmail(string email)
    {
        var input = RootElement.FindElement(EmailInputLocator);
        input.Clear();
        input.SendKeys(email);

        return this;
    }

    /// <summary>
    /// Clears the password field and types the given password.
    /// </summary>
    [AllureStep("Enter password")]
    public SignInModalComponent EnterPassword(string password)
    {
        var inputs = RootElement.FindElements(PasswordInputLocator);

        if (inputs.Count == 0)
        {
            throw new NoSuchElementException("Password input was not found.");
        }

        IWebElement? passwordInput = null;

        foreach (var input in inputs)
        {
            if (input.Displayed)
            {
                passwordInput = input;
                break;
            }
        }

        if (passwordInput == null)
        {
            throw new NoSuchElementException("Visible password input was not found.");
        }

        passwordInput.Click();
        passwordInput.SendKeys(password);

        return this;
    }

    /// <summary>
    /// Clicks the eye icon to toggle password visibility.
    /// </summary>
    [AllureStep("Toggle password visibility")]
    public void TogglePasswordVisibility()
    {
        RootElement.FindElement(PasswordTogglerLocator).Click();
    }

    /// <summary>
    /// Waits until the Sign In button is enabled and visible, then clicks it.
    /// </summary>
    [AllureStep("Click Sign In button")]
    public void ClickSignIn()
    {
        wait.Until(_ =>
        {
            var button = RootElement.FindElement(SignInButtonLocator);
            return button.Enabled && button.Displayed;
        });

        RootElement.FindElement(SignInButtonLocator).Click();
    }

    [AllureStep("Click Sign In button and wait for modal to close")]
    public void ClickSignInAndWaitClose()
    {
        ClickSignIn();
        wait.Until(ExpectedConditions.StalenessOf(RootElement));
    }

    /// <summary>
    /// Clicks the "Forgot password?" link to navigate to password recovery.
    /// </summary>
    [AllureStep("Click 'Forgot password?' link")]
    public void ClickForgotPassword()
    {
        RootElement.FindElement(ForgotPasswordLocator).Click();
    }

    /// <summary>
    /// Clicks the close button and waits for this specific modal instance to disappear.
    /// </summary>
    [AllureStep("Close the modal")]
    public void CloseModal()
    {
        var currentModal = RootElement;
        currentModal.FindElement(CloseButtonLocator).Click();

        wait.Until(_ =>
        {
            try
            {
                return !currentModal.Displayed;
            }
            catch (StaleElementReferenceException)
            {
                return true;
            }
        });
    }

    /// <summary>
    /// Clicks the "Sign up" link at the bottom of the modal.
    /// </summary>
    [AllureStep("Click 'Sign up' link")]
    public void ClickSignUpLink()
    {
        RootElement.FindElement(SignUpLinkLocator).Click();
    }

    /// <summary>
    /// Performs a full login sequence: enters email, enters password, clicks Sign In.
    /// </summary>
    [AllureStep("Perform login with email: {email}")]
    public void Login(string email, string password)
    {
        EnterEmail(email);
        EnterPassword(password);
        ClickSignInAndWaitClose();
    }

    /// <summary>
    /// Returns the text content of the modal title element.
    /// </summary>
    [AllureStep("Get modal title text")]
    public string GetTitleText()
    {
        return RootElement.FindElement(TitleLocator).Text;
    }

    /// <summary>
    /// Returns true if the Sign In submit button is currently enabled.
    /// </summary>
    [AllureStep("Check if Sign In button is enabled")]
    public bool IsSignInButtonEnabled()
    {
        return RootElement.FindElement(SignInButtonLocator).Enabled;
    }

    /// <summary>
    /// Returns true if at least one instance of the modal is currently visible.
    /// </summary>
    [AllureStep("Check if sign-in modal is visible")]
    public bool IsModalVisible()
    {
        return driver.FindElements(RootLocator).Any(element => element.Displayed);
    }

    /// <summary>
    /// Clicks the "Sign in with Google" button to initiate OAuth flow.
    /// </summary>
    [AllureStep("Click 'Sign in with Google' button")]
    public void ClickGoogleSignIn()
    {
        RootElement.FindElement(GoogleButtonLocator).Click();
    }

    /// <summary>
    /// Returns the text content of the general error message element.
    /// </summary>
    [AllureStep("Get error message text")]
    public string GetErrorMessage()
    {
        var error = wait.Until(_ =>
        {
            var element = RootElement.FindElement(ErrorMessageLocator);
            return element.Displayed ? element : null;
        });

        return error.Text.Trim();
    }

    [AllureStep("Get email error message text")]
    public string GetEmailErrorMessage()
    {
        WaitUntilElementVisibleBy(EmailErrorLocator);
        return RootElement.FindElement(EmailErrorLocator).Text.Trim();
    }
}