# 🚀 Quick Start Guide

## Your Framework is Ready! ✅

This guide will help you start using your new Selenium test automation framework immediately.

---

## 📋 What You Have

✅ **13 Working Test Cases** discovered and ready to run  
✅ **Complete Page Object Model** architecture  
✅ **Reusable Component Library** (Header, Footer, Navigation)  
✅ **Professional Infrastructure** (BasePage, BaseTest, DriverFactory)  
✅ **Modern C# .NET 8** implementation  
✅ **Build Successful** - Zero compilation errors  

---

## 🎯 5-Minute Quick Start

### Step 1: Update Your Base URL

Open any test file (e.g., `Tests/HomePageTests.cs`) and update the URL:

```csharp
protected override void OnSetup()
{
    BaseUrl = "https://your-actual-website.com";  // ⚡ Change this
    _homePage = new HomePage(Driver!);
    _homePage.Open(BaseUrl);
}
```

### Step 2: Update Locators

Open `Pages/HomePage.cs` and replace example locators with your actual selectors:

```csharp
// Before (Example)
private By WelcomeBannerLocator => By.CssSelector(".welcome-banner");

// After (Your App)
private By WelcomeBannerLocator => By.Id("hero-section");
```

### Step 3: Update Component Locators

Open `Components/HeaderComponent.cs`:

```csharp
// Before (Example)
protected override By ComponentRoot => By.CssSelector("header.main-header");
private By LogoLocator => By.CssSelector("header .logo");

// After (Your App)
protected override By ComponentRoot => By.TagName("header");
private By LogoLocator => By.XPath("//header//img[@alt='Logo']");
```

### Step 4: Run Your Tests

**Option A: Visual Studio Test Explorer**
1. Go to `Test` → `Test Explorer` (Ctrl+E, T)
2. Click `Run All Tests` ▶
3. View results in real-time

**Option B: Command Line**
```powershell
# Run all tests
dotnet test

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"
```

---

## 📂 Your Project Structure

```
green_city_sh.Tests/
│
├── Drivers/              # Browser driver management
├── Infrastructure/       # ⭐ Core framework (BasePage, BaseTest)
├── Components/           # ⭐ Reusable UI components
├── Pages/                # ⭐ Page objects
└── Tests/                # ⭐ Your test classes
```

---

## 💡 Key Concepts

### 1️⃣ Creating a New Page

```csharp
using OpenQA.Selenium;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Components;

namespace green_city_sh.Tests.Pages;

public class ContactPage(IWebDriver driver) : BasePage(driver)
{
    // Reuse existing components
    public HeaderComponent Header { get; } = new(driver);

    // Page-specific locators
    private By NameInputLocator => By.Id("name");
    private By EmailInputLocator => By.Id("email");
    private By SubmitButtonLocator => By.CssSelector("button[type='submit']");

    // Page-specific actions
    public void FillContactForm(string name, string email)
    {
        SendKeys(NameInputLocator, name);
        SendKeys(EmailInputLocator, email);
        Click(SubmitButtonLocator);
    }
}
```

### 2️⃣ Creating a New Component

```csharp
using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class ModalComponent(IWebDriver driver) : BaseComponent(driver)
{
    protected override By ComponentRoot => By.CssSelector(".modal");

    private By CloseButtonLocator => By.CssSelector(".modal-close");

    public void CloseModal()
    {
        Click(CloseButtonLocator);
    }

    public bool IsModalOpen()
    {
        return IsComponentDisplayed();
    }
}
```

### 3️⃣ Writing a New Test

```csharp
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;

namespace green_city_sh.Tests.Tests;

[TestFixture]
public class ContactPageTests : BaseTest
{
    private ContactPage? _contactPage;

    protected override void OnSetup()
    {
        BaseUrl = "https://your-site.com";
        _contactPage = new ContactPage(Driver!);
        _contactPage.Open($"{BaseUrl}/contact");
    }

    [Test]
    [Category("ContactForm")]
    public void VerifyContactFormSubmission()
    {
        _contactPage!.FillContactForm("John Doe", "john@example.com");

        Assert.That(_contactPage.GetPageTitle(), 
            Does.Contain("Thank You"));
    }
}
```

---

## 🔧 Common Tasks

### Change Browser Type

In `BaseTest.cs` or individual test:

```csharp
[SetUp]
public void Setup()
{
    // Chrome (default)
    Driver = DriverFactory.CreateDriver(BrowserType.Chrome);

    // Or Firefox
    // Driver = DriverFactory.CreateDriver(BrowserType.Firefox);

    // Or Edge
    // Driver = DriverFactory.CreateDriver(BrowserType.Edge);
}
```

### Add Screenshot on Every Test

```csharp
[TearDown]
public void TearDown()
{
    OnTearDown();

    // Always take screenshot (not just on failure)
    TakeScreenshot(TestContext.CurrentContext.Test.Name);

    Driver?.Quit();
}
```

### Add Test Categories

```csharp
[Test]
[Category("Smoke")]
[Category("Critical")]
public void MyTest() { }
```

Run by category:
```powershell
dotnet test --filter "Category=Smoke"
```

---

## 📊 Test Discovery

Your framework includes **13 sample tests**:

### HomePageTests (8 tests)
- ✅ VerifyHomePageLoads
- ✅ VerifyHeaderComponentIsDisplayed
- ✅ VerifyFooterComponentIsDisplayed
- ✅ VerifyNavigationComponentIsDisplayed
- ✅ VerifyAllComponentsAreDisplayed
- ✅ VerifySearchFunctionality
- ✅ VerifyNavigationToAboutPage
- ✅ VerifyNavigationToContactViaFooter

### ComponentTests (5 tests)
- ✅ VerifyHeaderComponentElements
- ✅ VerifyFooterComponentElements
- ✅ VerifyNavigationComponentElements
- ✅ VerifyHeaderSearchFunctionality
- ✅ VerifyNavigationMenuClicks

---

## 🎨 BasePage Methods You Can Use

```csharp
// Element interactions
Click(locator)
SendKeys(locator, text)
GetText(locator)
IsElementDisplayed(locator)

// Waits
WaitForElementToBeVisible(locator)
WaitForElementToBeInvisible(locator)
WaitForPageToLoad()

// Advanced
ScrollToElement(locator)
HoverOverElement(locator)
SelectDropdownByText(locator, text)
SelectDropdownByValue(locator, value)

// Navigation
NavigateToUrl(url)
GetCurrentUrl()
GetPageTitle()
```

---

## 🐛 Debugging Tips

### 1. See What Selenium is Doing
Add delays to watch the browser:
```csharp
Click(locator);
Thread.Sleep(2000);  // Wait 2 seconds
```

### 2. Check Screenshot Folder
Failed tests automatically save screenshots to:
```
green_city_sh.Tests/bin/Debug/net8.0/Screenshots/
```

### 3. Use Verbose Logging
```powershell
dotnet test --logger "console;verbosity=detailed"
```

### 4. Run Single Test
```powershell
dotnet test --filter "FullyQualifiedName~VerifyHomePageLoads"
```

---

## 📚 Documentation Files

| File | Purpose |
|------|---------|
| **IMPLEMENTATION_SUMMARY.md** | Complete feature overview |
| **PROJECT_STRUCTURE.md** | Architecture & design patterns |
| **README.md** | Comprehensive documentation |
| **This file** | Quick start guide |

---

## ✅ Pre-Flight Checklist

Before running tests on your actual application:

- [ ] Updated `BaseUrl` in test classes
- [ ] Updated page locators in `Pages/` folder
- [ ] Updated component locators in `Components/` folder
- [ ] Build successful (`dotnet build`)
- [ ] Tests discovered in Test Explorer
- [ ] Browser drivers work (automatic with WebDriverManager)

---

## 🎯 Next Steps

1. **Update one page** - Start with HomePage.cs
2. **Update one component** - Start with HeaderComponent.cs
3. **Run one test** - Start with VerifyHomePageLoads
4. **Iterate** - Fix locators as needed
5. **Expand** - Add more pages and tests

---

## 💬 Need Help?

### Common Issues:

**Q: Tests fail with "element not found"**  
A: Update the locators in your Page/Component classes to match your application

**Q: Chrome driver not found**  
A: Framework uses WebDriverManager - it auto-downloads drivers on first run

**Q: Build fails**  
A: Run `dotnet restore` then `dotnet build`

**Q: Tests not discovered**  
A: Rebuild solution (Ctrl+Shift+B) and refresh Test Explorer

---

## 🎉 You're Ready!

Your professional Selenium test automation framework is **production-ready**!

Start by updating the locators for your application and running your first test.

**Happy Testing!** 🚀

---

**Framework Version:** 1.0  
**Last Updated:** 2024  
**Build Status:** ✅ SUCCESS
