# Green City Test Automation - Project Structure

## 📊 Directory Tree

```
green_city_sh.Tests/
│
├── 📁 Drivers/
│   └── DriverFactory.cs                    # Browser driver factory (Chrome, Firefox, Edge)
│                                           # - Automatic driver setup using WebDriverManager
│                                           # - Browser configuration and options
│                                           # - Implicit and explicit wait configuration
│
├── 📁 Infrastructure/
│   ├── BasePage.cs                         # 🎯 Core page interaction layer
│   │                                       # - WebDriver wrapper methods
│   │                                       # - Explicit wait strategies
│   │                                       # - Element interaction methods (Click, SendKeys, GetText)
│   │                                       # - Scroll, hover, dropdown operations
│   │                                       # - JavaScript executor methods
│   │
│   ├── BaseTest.cs                         # 🎯 Test lifecycle management
│   │                                       # - [SetUp] / [TearDown] implementation
│   │                                       # - Driver initialization & cleanup
│   │                                       # - Screenshot capture on failure
│   │                                       # - Extensible test hooks
│   │
│   ├── Configuration.cs                    # Configuration and environment settings
│   │                                       # - Environment variable management
│   │                                       # - Default timeouts and URLs
│   │
│   └── WebElementExtensions.cs             # Extension methods for IWebElement
│                                           # - Retry logic for clicks
│                                           # - Element presence/visibility checks
│
├── 📁 Components/
│   ├── BaseComponent.cs                    # Base class for all reusable components
│   │                                       # - Component-level operations
│   │                                       # - Component visibility verification
│   │
│   ├── HeaderComponent.cs                  # 🎯 Header component (Example)
│   │                                       # - Logo interaction
│   │                                       # - Search functionality
│   │                                       # - Login/User menu
│   │
│   ├── FooterComponent.cs                  # Footer component
│   │                                       # - Copyright text
│   │                                       # - Social media links
│   │                                       # - Legal links (Privacy, Terms)
│   │
│   └── NavigationComponent.cs              # Navigation menu component
│                                           # - Menu item interactions
│                                           # - Dynamic menu navigation
│
├── 📁 Pages/
│   ├── HomePage.cs                         # 🎯 Home page with integrated components
│   │                                       # - Uses HeaderComponent
│   │                                       # - Uses FooterComponent
│   │                                       # - Uses NavigationComponent
│   │                                       # - Page-specific elements and actions
│   │
│   └── LoginPage.cs                        # Login page
│                                           # - Form interactions
│                                           # - Error message handling
│                                           # - Component integration example
│
├── 📁 Tests/
│   ├── HomePageTests.cs                    # Home page test suite
│   │                                       # - Page load verification
│   │                                       # - Component integration tests
│   │                                       # - Search and navigation tests
│   │
│   └── ComponentTests.cs                   # Component-specific tests
│                                           # - Header component tests
│                                           # - Footer component tests
│                                           # - Navigation component tests
│
├── green_city_sh.Tests.csproj              # Project file with NuGet packages
└── README.md                               # 📖 Project documentation
```

## 🔄 Architecture Flow

```
┌─────────────────────────────────────────────────────────────────┐
│                         Test Layer                              │
│  (HomePageTests.cs, ComponentTests.cs)                         │
│  - Inherits from BaseTest                                       │
│  - Uses Page Objects                                            │
└────────────────┬────────────────────────────────────────────────┘
                 │
                 ▼
┌─────────────────────────────────────────────────────────────────┐
│                      Page Object Layer                          │
│  (HomePage.cs, LoginPage.cs)                                   │
│  - Inherits from BasePage                                       │
│  - Composes Components                                          │
│  - Exposes page-specific methods                                │
└────────────────┬────────────────────────────────────────────────┘
                 │
                 ├──────────────────┬──────────────────────────────┐
                 ▼                  ▼                              ▼
┌─────────────────────┐  ┌──────────────────────┐  ┌─────────────────────┐
│  HeaderComponent    │  │  FooterComponent     │  │ NavigationComponent │
│  (HeaderComponent.cs)  │  (FooterComponent.cs)│  │ (NavigationComponent.cs)
│  - Inherits from    │  │  - Inherits from     │  │ - Inherits from     │
│    BaseComponent    │  │    BaseComponent     │  │   BaseComponent     │
│  - Logo, Search     │  │  - Links, Copyright  │  │ - Menu items        │
└─────────────────────┘  └──────────────────────┘  └─────────────────────┘
                 │                  │                              │
                 └──────────────────┴──────────────────────────────┘
                                    ▼
┌─────────────────────────────────────────────────────────────────┐
│                   Infrastructure Layer                          │
│                                                                 │
│  ┌──────────────┐  ┌────────────┐  ┌───────────────────────┐  │
│  │  BasePage    │  │  BaseTest  │  │  DriverFactory        │  │
│  │              │  │            │  │                       │  │
│  │ - WebDriver  │  │ - Setup    │  │ - CreateDriver()      │  │
│  │   methods    │  │ - TearDown │  │ - Browser config      │  │
│  │ - Waits      │  │ - Driver   │  │ - WebDriverManager    │  │
│  └──────────────┘  └────────────┘  └───────────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
                                    ▼
┌─────────────────────────────────────────────────────────────────┐
│                     Selenium WebDriver                          │
│                    (Browser Automation)                         │
└─────────────────────────────────────────────────────────────────┘
```

## 🎯 Component Integration Example

**HomePage.cs integrating HeaderComponent:**

```csharp
public class HomePage(IWebDriver driver) : BasePage(driver)
{
    // Component instantiation
    public HeaderComponent Header { get; } = new(driver);

    // Using component in page methods
    public void SearchFromHomePage(string searchTerm)
    {
        Header.SearchFor(searchTerm);  // Delegates to component
    }
}
```

**Test using the integrated component:**

```csharp
[Test]
public void VerifySearchFunctionality()
{
    _homePage.SearchFromHomePage("test");  // Clean, readable API
    Assert.That(_homePage.GetPageTitle(), Does.Contain("Results"));
}
```

## 📐 Design Patterns Used

1. **Page Object Model (POM)** - Pages represent web pages
2. **Component Object Model (COM)** - Reusable UI components
3. **Factory Pattern** - DriverFactory for WebDriver creation
4. **Template Method Pattern** - BaseTest with extensible hooks
5. **Composition over Inheritance** - Pages compose components
6. **Primary Constructors** - Modern C# syntax for DI

## 🔑 Key Benefits

✅ **Maintainability** - Changes in one component affect all pages using it
✅ **Reusability** - Components can be shared across multiple pages
✅ **Readability** - Clean, fluent API for test writers
✅ **Scalability** - Easy to add new pages and components
✅ **Type Safety** - Compile-time checking with strong typing
✅ **Modern C#** - File-scoped namespaces, primary constructors

---

**Framework Version:** 1.0
**Last Updated:** 2024
**Target Framework:** .NET 8
