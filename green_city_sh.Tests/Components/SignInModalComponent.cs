using green_city_sh.Tests.Infrastructure;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace green_city_sh.Tests.Components;

public class SignInModalComponent : BaseComponent
{
    /// <summary>CSS locator for the modal root element.</summary>
    public static readonly By RootLocator = By.CssSelector("app-auth-modal");
    /// <summary>
    /// Initializes the component by locating the root element via CSS locator.
    /// </summary>
    public SignInModalComponent(IWebDriver driver, By rootLocator)
        : base(driver, rootLocator)
    { }
    /// <summary>
    /// Initializes the component from an already-resolved root element.
    /// </summary>
    public SignInModalComponent(IWebDriver driver, IWebElement rootElement)
        : base(driver, rootElement)
    { }

    /// <summary>
    /// Waits up to 10 seconds for the sign-in modal to become visible,
    /// then returns a component bound to that exact DOM element
    /// </summary>
    /// <param name="driver">The active WebDriver session.</param>
    /// <returns>a <see cref="SignInModalComponent"/> bound to the visible modal.</returns>
    /// <exception cref="WebDriverTimeoutException">Thrown if no visible modal appears withing the timeout.</exception>
    public static SignInModalComponent WaitAndCreate(IWebDriver driver)
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
            ?? throw new WebDriverTimeoutException("Sign-in modal did not become visible.");
        return new SignInModalComponent(driver, modalRoot);
    }

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

    /// <summary>
    /// Clears the email field and types the given email address.
    /// </summary>
    /// <param name="email">Email address to enter.</param>
    public void EnterEmail(string email)
    {
        var input = RootElement.FindElement(EmailInputLocator);
        input.Clear();
        input.SendKeys(email);
    }

    /// <summary>
    /// Clears the password field and types the given password.
    /// Matches both <c>type='password'</c> and <c>type='text'</c> to handle
    /// the state after <see cref="TogglePasswordVisibility"/> is called.
    /// </summary>
    /// <param name="password">Password to enter</param>
    /// <exception cref="NoSuchElementException">
    /// Thrown if neither password nor text input is found inside the modal
    /// </exception>
    public void EnterPassword(string password)
    {
        var inputs = RootElement.FindElements(PasswordInputLocator);
        if (inputs.Count == 0)
            throw new NoSuchElementException("Password input was not found.");
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
            throw new NoSuchElementException("Visible password input was not found.");

        passwordInput.Click();
        passwordInput.SendKeys(password);
    }

    /// <summary>
    /// Clicks the eye icon to toggle password visibility.
    /// </summary>
    public void TogglePasswordVisibility() => RootElement.FindElement(PasswordTogglerLocator).Click();

    /// <summary>
    /// Waits until the Sign In button is enabled and visible, then clicks it.
    /// Satisfies the acceptance criterion: button must not be disabled before click.
    /// </summary>
    public void ClickSignIn()
    {
        wait
            .Until(_ =>
            {
                var btn = RootElement.FindElement(SignInButtonLocator);
                return btn.Enabled && btn.Displayed;
            });
        RootElement.FindElement(SignInButtonLocator).Click();
    }

    /// <summary>
    /// Clicks the "Forgot password?" link to navigate to password recovery.
    /// </summary>
    public void ClickForgotPassword() => RootElement.FindElement(ForgotPasswordLocator).Click();

    /// <summary>
    /// Clicks the x close button and waits for this specific modal instance to disappear from the DOM.
    /// Handles <see cref="StaleElementReferenceException"/>
    /// as a successful close - the element was removed before the check ran.
    /// </summary>
    public void CloseModal()
    {
        var currentModal = RootElement;
        currentModal.FindElement(CloseButtonLocator).Click();

        wait
            .Until(_ =>
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
    public void ClickSignUpLink() => RootElement.FindElement(SignUpLinkLocator).Click();

    /// <summary>
    /// Performs a full login sequence: enters email, enters password, clicks Sign In.
    /// </summary>
    /// <param name="email">User's email address.</param>
    /// <param name="password">User's password.</param>
    public void Login(string email, string password)
    {
        EnterEmail(email);
        EnterPassword(password);
        ClickSignIn();
    }

    /// <summary>
    /// Returns the text content of the modal title element.
    /// </summary>
    public string GetTitleText() => RootElement.FindElement(TitleLocator).Text;

    /// <summary>
    /// Returns true if the Sign In submit button is currently enabled.
    /// </summary>
    /// <returns></returns>
    public bool IsSignInButtonEnabled() => RootElement.FindElement(SignInButtonLocator).Enabled;

    /// <summary>
    /// Returns true if at least one instance of the modal is currently visible.
    /// Check all DOM matches to avoid false negatives from hidden duplicates.
    /// </summary>
    /// <returns></returns>
    public bool IsModalVisible()
    {
        return driver.FindElements(RootLocator).Any(e => e.Displayed);
    }

    /// <summary>
    /// Clicks the "Sign in with Google" button to initiate OAuth flow.
    /// </summary>
    public void ClickGoogleSignIn() => RootElement.FindElement(GoogleButtonLocator).Click();

    /// <summary>
    /// Returns the error message text for invalid email format.
    /// </summary>
    public string GetEmailErrorMessage()
    {
        return RootElement.FindElement(EmailErrorLocator).Text;
    }

}
