using OpenQA.Selenium;

namespace green_city_sh.Tests.Infrastructure;

public static class WebElementExtensions
{
    public static bool IsElementPresent(this IWebDriver driver, By locator)
    {
        try
        {
            driver.FindElement(locator);
            return true;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }

    public static bool IsElementVisible(this IWebDriver driver, By locator)
    {
        try
        {
            var element = driver.FindElement(locator);
            return element.Displayed;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }

    public static void ClickWithRetry(this IWebElement element, int maxRetries = 3)
    {
        for (int i = 0; i < maxRetries; i++)
        {
            try
            {
                element.Click();
                return;
            }
            catch (ElementClickInterceptedException)
            {
                if (i == maxRetries - 1) throw;
                Thread.Sleep(500);
            }
        }
    }

    public static void ClearAndSendKeys(this IWebElement element, string text)
    {
        element.Clear();
        element.SendKeys(text);
    }
}
