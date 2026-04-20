using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace green_city_sh.Tests.Components;

public class SignInModalComponent : BaseComponent
{
    public static readonly By RootLocator = By.CssSelector("app-auth-modal");
    public SignInModalComponent(IWebDriver driver, By rootLocator)
        : base(driver, rootLocator)
    { }

    public SignInModalComponent(IWebDriver driver, IWebElement rootElement)
        : base(driver, rootElement)
    { }
    
    public static SignInModalComponent WaitAndCreate(IWebDriver driver)
    {
        var modalRoot = new WebDriverWait(driver, TimeSpan.FromSeconds(10))
            .Until(ExpectedConditions.ElementIsVisible(RootLocator));
        return new SignInModalComponent(driver, modalRoot);
    }
    
    private static readonly By TitleLocator = By.CssSelector("h1");
    private static readonly By EmailInputLocator = By.CssSelector("input[type='email']");
    private static readonly By PasswordInputLocator = By.CssSelector("input[type='password']");
    private static readonly By PasswordTogglerLocator = By.CssSelector("img[src*='eye'], img[class*='eye']");
    private static readonly By SignInButtonLocator = By.CssSelector("button[type='submit']");
    private static readonly By GoogleButtonLocator = By.XPath(".//button[contains(., 'Google')]");
    private static readonly By CloseButtonLocator = By.CssSelector("a.close-modal-window");
    private static readonly By ForgotPasswordLocator = By.XPath(".//a[contains(text(), 'Forgot password')]");
    private static readonly By SignUpLinkLocator = By.XPath(".//a[contains(text(), 'Sign up')]");

    public void EnterEmail(string email)
    {
        var input = RootElement.FindElement(EmailInputLocator);
        input.Clear();
        input.SendKeys(email);
    }

    public void EnterPassword(string password)
    {
        var input = RootElement.FindElement(PasswordInputLocator);
        input.Clear();
        input.SendKeys(password);
    }

    public void TogglePasswordVisibility() => RootElement.FindElement(PasswordTogglerLocator).Click();

    public void ClickSignIn()
    {
        new WebDriverWait(driver, TimeSpan.FromSeconds(5))
            .Until(_ =>
            {
                var btn = RootElement.FindElement(SignInButtonLocator);
                return btn.Enabled && btn.Displayed;
            });
        RootElement.FindElement(SignInButtonLocator).Click();
    }
    
    public void ClickForgotPassword() => RootElement.FindElement(ForgotPasswordLocator).Click();

    public void CloseModal()
    {
        RootElement.FindElement(CloseButtonLocator).Click();
        
        new WebDriverWait(driver, TimeSpan.FromSeconds(5))
            .Until(ExpectedConditions.InvisibilityOfElementLocated(RootLocator));
    }
    
    public void ClickSignUpLink() => RootElement.FindElement(SignUpLinkLocator).Click();

    public void Login(string email, string password)
    {
        EnterEmail(email);
        EnterPassword(password);
        ClickSignIn();
    }
    
    public string GetTitleText() => RootElement.FindElement(TitleLocator).Text;
    public bool IsSignInButtonEnabled() => RootElement.FindElement(SignInButtonLocator).Enabled;

    public bool IsModalVisible()
    {
        return driver.FindElements(RootLocator).Any(e => e.Displayed);
    }
    
    public void ClickGoogleSignIn() => RootElement.FindElement(GoogleButtonLocator).Click();

}
