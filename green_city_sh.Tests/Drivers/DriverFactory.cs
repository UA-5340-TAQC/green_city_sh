using green_city_sh.Tests.Infrastructure;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace green_city_sh.Tests.Drivers;

public static class DriverFactory
{
    public static IWebDriver CreateDriver(BrowserType browserType = BrowserType.Chrome)
    {
        IWebDriver driver = browserType switch
        {
            BrowserType.Chrome => CreateChromeDriver(),
            BrowserType.Firefox => CreateFirefoxDriver(),
            BrowserType.Edge => CreateEdgeDriver(),
            _ => throw new ArgumentException($"Browser type {browserType} is not supported")
        };

        driver.Manage().Window.Maximize();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Configuration.DefaultTimeout);
        driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(Configuration.PageLoadTimeout);

        return driver;
    }

    private static IWebDriver CreateChromeDriver()
    {
        //new DriverManager().SetUpDriver(new ChromeConfig());
        new DriverManager().SetUpDriver(new ChromeConfig(), "147.0.7727.117");
        var options = new ChromeOptions();
        options.AddArgument("--start-maximized");
        options.AddArgument("--disable-notifications");
        options.AddArgument("--disable-popup-blocking");
        return new ChromeDriver(options);
    }

    private static IWebDriver CreateFirefoxDriver()
    {
        new DriverManager().SetUpDriver(new FirefoxConfig());
        var options = new FirefoxOptions();
        return new FirefoxDriver(options);
    }

    private static IWebDriver CreateEdgeDriver()
    {
        new DriverManager().SetUpDriver(new EdgeConfig());
        var options = new EdgeOptions();
        options.AddArgument("--start-maximized");
        return new EdgeDriver(options);
    }
}

public enum BrowserType
{
    Chrome,
    Firefox,
    Edge
}
