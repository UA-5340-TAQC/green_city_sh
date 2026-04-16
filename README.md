# Selenium WebDriver Test Automation Framework

## 📋 Overview
This is a professional test automation framework built with **Selenium WebDriver**, **C# .NET 8**, and **NUnit** for the Green City project.

## 🏗️ Architecture

The framework implements:
- **Page Object Model (POM)** for page representations
- **Component Object Model (COM)** for reusable UI components
- **BasePage** for common WebDriver interactions
- **BaseTest** for test initialization and cleanup
- **DriverFactory** for WebDriver management

## 📂 Project Structure

```
green_city_sh.Tests/
│
├── Drivers/
│   └── DriverFactory.cs           # WebDriver factory with browser support
│
├── Infrastructure/
│   ├── BasePage.cs                # Base class with common WebDriver methods
│   ├── BaseTest.cs                # Base test class with Setup/TearDown
│   ├── Configuration.cs           # Configuration management
│   └── WebElementExtensions.cs    # Extension methods for IWebElement
│
├── Components/
│   ├── BaseComponent.cs           # Base class for all components
│   ├── HeaderComponent.cs         # Header component (logo, search, login)
│   ├── FooterComponent.cs         # Footer component (links, copyright)
│   └── NavigationComponent.cs     # Navigation menu component
│
├── Pages/
│   ├── HomePage.cs                # Home page with integrated components
│   └── LoginPage.cs               # Login page implementation
│
└── Tests/
    ├── HomePageTests.cs           # Tests for Home page
    └── ComponentTests.cs          # Tests for reusable components
```

## 🎯 Key Features

### 1. **BasePage.cs**
Provides common WebDriver interactions:
- Element finding with explicit waits
- Click, SendKeys, GetText operations
- Scrolling and hovering
- Dropdown selection
- Visibility checks

### 2. **BaseTest.cs**
Handles test lifecycle:
- Driver initialization in `[SetUp]`
- Screenshot capture on test failure
- Driver cleanup in `[TearDown]`
- Extensible hooks (`OnSetup`, `OnTearDown`)

### 3. **Component Integration**
Example: **HeaderComponent** in **HomePage**
```csharp
public class HomePage(IWebDriver driver) : BasePage(driver)
{
    public HeaderComponent Header { get; } = new(driver);

    public void SearchFromHomePage(string searchTerm)
    {
        Header.SearchFor(searchTerm);
    }
}
```

### 4. **DriverFactory**
Supports multiple browsers:
- Chrome (default)
- Firefox
- Edge

Automatic driver management using WebDriverManager.

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- Visual Studio 2022/2026
- Chrome/Firefox/Edge browser

### Installation

1. Restore NuGet packages:
```bash
dotnet restore
```

2. Build the solution:
```bash
dotnet build
```

### Running Tests

**Run all tests:**
```bash
dotnet test
```

**Run tests by category:**
```bash
dotnet test --filter "Category=Smoke"
dotnet test --filter "Category=Component"
```

**Run specific test:**
```bash
dotnet test --filter "FullyQualifiedName~HomePageTests.VerifyHomePageLoads"
```

## 📦 NuGet Packages

- **Selenium.WebDriver** (4.25.0) - WebDriver API
- **Selenium.Support** (4.25.0) - Support classes
- **WebDriverManager** (2.17.4) - Automatic driver management
- **NUnit** (3.14.0) - Test framework
- **NUnit3TestAdapter** (4.5.0) - Test adapter

## 🔧 Configuration

Update `BaseUrl` in test classes or use environment variables:

```csharp
protected override void OnSetup()
{
    BaseUrl = "https://your-app-url.com";
    base.OnSetup();
}
```

Or set environment variables:
- `BASE_URL` - Application URL
- `BROWSER` - Browser type (Chrome/Firefox/Edge)
- `DEFAULT_TIMEOUT` - Wait timeout in seconds
- `HEADLESS` - Run in headless mode (true/false)

## 📝 Writing Tests

### Example Test:
```csharp
[TestFixture]
public class MyTests : BaseTest
{
    private HomePage? _homePage;

    protected override void OnSetup()
    {
        _homePage = new HomePage(Driver!);
        _homePage.Open(BaseUrl);
    }

    [Test]
    public void VerifySearch()
    {
        _homePage!.SearchFromHomePage("test");
        Assert.That(_homePage.GetPageTitle(), Does.Contain("Results"));
    }
}
```

## 🎨 Modern C# Features Used

- **File-scoped namespaces** - Cleaner code structure
- **Primary constructors** - Simplified class initialization
- **Null-forgiving operator** - Null safety
- **Pattern matching** - Switch expressions
- **Top-level statements** - Reduced boilerplate

## 📸 Screenshots

Failed tests automatically capture screenshots to `Screenshots/` folder.

## 🤝 Contributing

1. Follow the existing code structure
2. Add tests for new features
3. Use meaningful test names
4. Add appropriate `[Category]` attributes

## 📄 License

This project is part of the Green City initiative.

---

**Happy Testing! 🧪**
