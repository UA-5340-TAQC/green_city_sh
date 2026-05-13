# 🌿 Green City - Test Automation Framework

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Selenium](https://img.shields.io/badge/Selenium-4.25.0-43B02A?logo=selenium)](https://www.selenium.dev/)
[![NUnit](https://img.shields.io/badge/NUnit-3.14.0-22B14C?logo=nunit)](https://nunit.org/)
[![C#](https://img.shields.io/badge/C%23-12.0-239120?logo=csharp)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![RestSharp](https://img.shields.io/badge/RestSharp-114.0.0-0078D7?logo=rest)](https://restsharp.dev/)
[![Allure](https://img.shields.io/badge/Allure-2.15.0-FFA500?logo=qameta)](https://docs.qameta.io/allure/)
[![Build](https://img.shields.io/badge/Build-Passing-brightgreen)]()

## 📋 Overview

<a href="https://ua-5340-taqc.github.io/green_city_sh/" target="_blank">
    <img src="https://img.shields.io/badge/Allure%20Report-main-blue" alt="Allure Report"/>
</a>

**Professional Test Automation Framework** for the [**Green City**](https://www.greencity.cx.ua/#/greenCity) ecological platform. Built with **Selenium WebDriver**, **C# .NET 8**, and **NUnit**, implementing enterprise-grade design patterns for maintainable and scalable test automation. The framework supports both **UI (Web)** and **API** testing with comprehensive coverage.

**Green City** is an eco-friendly social platform that promotes sustainable living, environmental awareness, and green habits. This framework ensures quality and reliability through comprehensive automated testing across all application layers.

### 🎯 Project Goals
- ✅ Comprehensive UI and API test coverage for Green City platform
- ✅ Cross-browser compatibility testing (Chrome, Firefox, Edge)
- ✅ REST API testing with RestSharp
- ✅ Maintainable test code using industry-standard design patterns
- ✅ CI/CD ready with automated test execution via GitHub Actions
- ✅ Reusable components for rapid test development
- ✅ Allure reporting with automatic screenshots on failure
- ✅ Parallel test execution support

### 🌐 Application Under Test
- **URL:** [https://www.greencity.cx.ua/#/greenCity](https://www.greencity.cx.ua/#/greenCity)
- **Platform:** Web-based ecological social network
- **Technologies:** Angular SPA with responsive design
- **Features:** Events, News, Profile Management, Eco-tips, Habits, Friends
- **Testing Layers:** UI (Selenium WebDriver) + REST API (RestSharp)

## 🏗️ Architecture & Design Patterns

The framework implements **enterprise-grade design patterns** for maximum maintainability and scalability:

| Pattern | Implementation | Purpose | Count |
|---------|---------------|---------|-------|
| **Page Object Model (POM)** | `Pages/` folder | Encapsulate page elements and actions | 14 Pages |
| **Component Object Model (COM)** | `Components/` folder | Reusable UI components | 40 Components |
| **Modal Object Model** | `Modals/` folder | Dialog and popup windows | 4 Modals |
| **API Client Pattern** | `Api/Clients/` folder | REST API service clients | 11 Clients |
| **DTO Pattern** | `Api/DTO/` folder | Data transfer objects for API | 20+ DTOs |
| **Factory Pattern** | `DriverFactory.cs` | Centralized WebDriver creation | ✓ |
| **Template Method** | `BaseUITest.cs` / `BaseAPITest.cs` | Test lifecycle with extensible hooks | ✓ |
| **Inheritance Hierarchy** | `Base → BasePage/BaseComponent/BaseModal` | Shared WebDriver functionality | ✓ |
| **Composition** | Pages compose Components & Modals | Flexible page architecture | ✓ |

### 🔧 Core Framework Architecture

```
Infrastructure/
│
├── Base.cs (Abstract Root)
│   ├── IWebDriver driver
│   ├── WebDriverWait wait
│   └── Configuration.DefaultTimeout
│
├── BasePage.cs extends Base
│   ├── Open(url)
│   ├── GetTitle()
│   └── GetUrl()
│
├── BaseComponent.cs extends Base
│   ├── IWebElement RootElement
│   └── Locator-based or Element-based construction
│
├── BaseModal.cs extends Base
│   ├── Modal dialog operations
│   └── Overlay and popup handling
│
├── BaseUITest.cs (NUnit TestFixture)
│   ├── [SetUp] → Setup()
│   ├── [TearDown] → TearDown()
│   ├── OnSetup() hook
│   ├── OnTearDown() hook
│   ├── TakeScreenshot() on failure
│   └── Allure reporting integration
│
└── BaseAPITest.cs (NUnit TestFixture)
    ├── Parallel execution support
    └── Allure reporting integration

Api/
│
├── Clients/ (REST API Clients)
│   ├── BaseApiClient.cs (HTTP operations)
│   ├── EcoNewsClient.cs
│   ├── EventsClient.cs
│   ├── HabitClient.cs
│   ├── FriendClient.cs
│   └── SearchClient.cs
│
└── DTO/ (Data Transfer Objects)
    ├── Request models
    └── Response models
```

**Key Framework Classes:**

| Class | Purpose | Location |
|-------|---------|----------|
| **`Base.cs`** | Root abstraction with WebDriver and WebDriverWait | `Infrastructure/` |
| **`BasePage.cs`** | Page-level operations (navigation, title, URL) | `Pages/` |
| **`BaseComponent.cs`** | Component operations with RootElement | `Components/` |
| **`BaseModal.cs`** | Modal dialog base operations | `Modals/` |
| **`BaseUITest.cs`** | NUnit UI test lifecycle management | `Infrastructure/` |
| **`BaseAPITest.cs`** | NUnit API test lifecycle management | `Infrastructure/` |
| **`BaseApiClient.cs`** | Base HTTP client for REST API calls | `Api/Clients/` |
| **`DriverFactory.cs`** | Multi-browser WebDriver factory | `Drivers/` |
| **`Configuration.cs`** | Centralized configuration settings | `Infrastructure/` |
| **`WebElementExtensions.cs`** | Extension methods for IWebElement | `Infrastructure/` |

## 📂 Project Structure

```
green_city_sh.Tests/
│
├── 📁 Drivers/
│   └── DriverFactory.cs                  # Multi-browser WebDriver factory
│
├── 📁 Infrastructure/
│   ├── Base.cs                           # Root class with WebDriver & Wait
│   ├── BasePage.cs                       # Page base (in Pages/ folder)
│   ├── BaseTest.cs                       # NUnit test lifecycle
│   ├── Configuration.cs                  # Settings management
│   └── WebElementExtensions.cs           # IWebElement utilities
│
```
green_city_sh.Tests/
│
├── 📁 Drivers/
│   └── DriverFactory.cs                  # Multi-browser WebDriver factory
│
├── 📁 Infrastructure/
│   ├── Base.cs                           # Root class with WebDriver & Wait
│   ├── BaseUITest.cs                     # NUnit UI test lifecycle
│   ├── BaseAPITest.cs                    # NUnit API test lifecycle
│   ├── Configuration.cs                  # Settings management (.env support)
│   └── WebElementExtensions.cs           # IWebElement utilities
│
├── 📁 Api/ (API Testing Layer)
│   ├── 📁 Clients/ (11 API Clients)
│   │   ├── BaseApiClient.cs              # Base HTTP client
│   │   ├── EcoNewsClient.cs              # EcoNews API operations
│   │   ├── EcoNewsCommentClient.cs       # EcoNews comments
│   │   ├── EventsClient.cs               # Events API
│   │   ├── EventCommentClient.cs         # Event comments
│   │   ├── HabitClient.cs                # Habits API
│   │   ├── HabitCommentClient.cs         # Habit comments
│   │   ├── FriendClient.cs               # Friends management
│   │   ├── OwnSecurityClient.cs          # Authentication
│   │   ├── SearchClient.cs               # Search places
│   │   └── SearchEventsClient.cs         # Search events
│   │
│   └── 📁 DTO/ (Data Transfer Objects)
│       ├── AuthResponce.cs               # Authentication response
│       ├── SignInModal.cs                # Sign-in request
│       ├── EcoNews/ (5 models)           # EcoNews DTOs
│       ├── Events/ (4 models)            # Events DTOs
│       ├── EcoNewsComment/ (2 models)    # Comment DTOs
│       ├── Habits/ (2 models)            # Habits DTOs
│       └── Search responses              # Search result DTOs
│
├── 📁 Components/ (40 Components)
│   ├── BaseComponent.cs                  # Component base class
│   ├── HeaderComponent.cs                # Site header
│   ├── FooterComponent.cs                # Site footer
│   ├── SignInModalComponent.cs           # Login modal
│   ├── SignUpModalComponent.cs           # Registration modal
│   ├── EventCardComponent.cs             # Event card
│   ├── EventsListComponent.cs            # Event list container
│   ├── EventDetailsCardComponent.cs      # Event details
│   ├── EventsFilterSectionComponent.cs   # Event filters
│   ├── EventsTopBarComponent.cs          # Events page header
│   ├── MyEventsTabComponent.cs           # User events tab
│   ├── BookmarkTabComponent.cs           # Bookmarks tab
│   ├── EditProfileButtonComponent.cs     # Edit profile button
│   ├── NewsFormComponent.cs              # News creation form
│   ├── NewsCardComponent.cs              # News card component
│   ├── NewsDetailsInfoComponent.cs       # News details info
│   ├── NewsFilterSelectionComponent.cs   # News filters
│   ├── NewsListComponent.cs              # News list
│   ├── NewsTopBarComponent.cs            # News top bar
│   ├── NewsTagsComponent.cs              # News tags
│   ├── NewsRichTextEditorComponent.cs    # Rich text editor
│   ├── NewsImageUploadComponent.cs       # Image uploader
│   ├── CommentComponent.cs               # Comment section
│   ├── CommentsComponent.cs              # Comments list
│   ├── TagsComponent.cs                  # Tags selector
│   ├── ImageUploadComponent.cs           # Image upload widget
│   ├── ConfirmationPopUpComponent.cs     # Confirmation dialog
│   ├── DateTimePickerComponent.cs        # Date/time picker
│   ├── DropDownComponent.cs              # Dropdown selector
│   ├── RichTextEditorComponent.cs        # Rich text editor
│   ├── MaterialCheckboxComponent.cs      # Material checkbox
│   ├── ProfileDetailsComponent.cs        # Profile details
│   ├── PrivacySettingsComponent.cs       # Privacy settings
│   ├── EmailNotificationsComponent.cs    # Email notifications
│   ├── SocialLinksComponent.cs           # Social media links
│   ├── UploadAvatarComponent.cs          # Avatar upload
│   ├── MyHabitsTabComponent.cs           # My habits tab
│   ├── MyNewsTabComponent.cs             # My news tab
│   ├── HabitCardComponent.cs             # Habit card
│   └── NewsInfoComponent.cs              # News info display
│
├── 📁 Pages/ (14 Page Objects)
│   ├── BasePage.cs                       # Page base class
│   ├── BaseNewsFormPage.cs               # News form base
│   ├── HomePage.cs                       # Main landing page
│   ├── EventsPage.cs                     # Events listing page
│   ├── EventDetailsPage.cs               # Single event details
│   ├── CreateEventPage.cs                # Event creation
│   ├── CreateUpdateEventPage.cs          # Event creation/editing
│   ├── NewsPage.cs                       # News listing page
│   ├── NewsDetailsPage.cs                # News article details
│   ├── CreateNewsPage.cs                 # News creation page
│   ├── MySpacePage.cs                    # User dashboard
│   ├── ProfileEditPage.cs                # Profile editing page
│   ├── SavedPage.cs                      # Saved items page
│   └── CreateHabitPage.cs                # Habit creation page
│
├── 📁 Modals/ (4 Modal Objects)
│   ├── BaseModal.cs                      # Modal base class
│   ├── UploadImageModal.cs               # Image upload dialog
│   ├── DeleteCommentModal.cs             # Delete confirmation
│   └── CancelJoiningEventModal.cs        # Cancel event joining
│
├── 📁 Tests/
│   ├── 📁 WEB/ (21 UI Test Files)
│   │   ├── HomePageTests.cs
│   │   ├── EventsSearchTests.cs
│   │   ├── EventPageTests.cs
│   │   ├── EventDetailsPageTests.cs
│   │   ├── CreateEventsTests.cs
│   │   ├── CreateEventButtonTest.cs
│   │   ├── SearchTypeAndDateRangeTest.cs
│   │   ├── NewsPageTests.cs
│   │   ├── NewsDetailsPageTests.cs
│   │   ├── CreateNewsImageUploadTests.cs
│   │   ├── CreateNewsTitleValidationTests.cs
│   │   ├── CreateNewsSourceValidationTests.cs
│   │   ├── LoginTests.cs
│   │   ├── LogoutTests.cs
│   │   ├── RegistrationTests.cs
│   │   ├── InvalidPasswordTests.cs
│   │   ├── ProfileTests.cs
│   │   ├── TC001_AddCommentTests.cs
│   │   ├── TC010_PublishButton_RemainsDisabled.cs
│   │   ├── TC011_CreateEventTest.cs
│   │   └── TC037_EventCardUITest.cs
│   │
│   ├── 📁 API/ (10 API Test Files)
│   │   ├── EcoNewsApiTests.cs
│   │   ├── EcoNewsCommentApiTests.cs
│   │   ├── EventsAPITests.cs
│   │   ├── EventCommentTests.cs
│   │   ├── HabitApiTests.cs
│   │   ├── HabitsCommentAPITests.cs
│   │   ├── FriendTests.cs
│   │   ├── LoginTests.cs
│   │   ├── SearchEventsAPITests.cs
│   │   └── SearchPlacesAPITests.cs
│   │
│   └── TestEnvironmentSetup.cs           # Environment setup configuration
│
├── 📁 TestData/
│   ├── validImage.jpg                    # Valid test image
│   ├── test_image.jpg                    # Test image file
│   └── invalidFile.pdf                   # Invalid file for negative tests
│
├── .env                                  # Environment variables
├── allureConfig.json                     # Allure configuration
└── green_city_sh.Tests.csproj            # Project file
```

## 📊 Framework Statistics

| Metric | Count | Status |
|--------|-------|--------|
| **Page Objects** | 14 | ✅ |
| **UI Components** | 40 | ✅ |
| **Modal Objects** | 4 | ✅ |
| **API Clients** | 11 | ✅ |
| **DTO Models** | 20+ | ✅ |
| **WEB UI Test Files** | 21 | ✅ |
| **API Test Files** | 10 | ✅ |
| **Total Test Files** | 31 | ✅ |
| **Infrastructure Classes** | 7 | ✅ |
| **CI/CD Pipeline** | GitHub Actions | ✅ |
| **Build Status** | Passing | ✅ |
| **Parallel Execution** | Enabled | ✅ |

## 🎯 Implemented Features

### ✅ Core Features
- **Multi-layer testing:** UI (Selenium) + API (RestSharp)
- **Multi-browser support:** Chrome, Firefox, Edge
- **Automatic WebDriver management:** WebDriverManager
- **Page Object Model:** 14 page objects
- **Component Object Model:** 40 reusable components
- **Modal Object Model:** 4 modal objects
- **API Client Layer:** 11 REST API clients
- **DTO Pattern:** 20+ data transfer objects
- **Explicit waits:** WebDriverWait for reliable synchronization
- **Screenshot capture:** Automatic on test failure
- **Allure reporting:** Rich test reports with history
- **Configuration management:** Environment variables with .env support
- **NUnit framework:** Industry-standard test framework
- **Parallel execution:** Tests run in parallel for speed
- **Test lifecycle hooks:** OnSetup, OnTearDown for customization
- **CI/CD Integration:** GitHub Actions workflow

### 🌐 Web UI Testing Coverage

**Authentication & User Management:**
- Login functionality
- Registration (sign up)
- Logout functionality
- Invalid password handling
- Profile viewing and editing

**Events Features:**
- Events listing and browsing
- Event search functionality
- Event details viewing
- Event creation
- Event filtering by type and date range
- Event card UI validation
- Create event button behavior

**News Features:**
- News listing and browsing
- News details viewing
- News creation
- News image upload validation
- News title validation
- News source validation
- Publish button state validation

**Comments & Interactions:**
- Adding comments
- Deleting comments
- Comment display validation

### 🔌 API Testing Coverage

**EcoNews API:**
- Create, read, update, delete eco-news articles
- EcoNews comments management
- EcoNews search and filtering

**Events API:**
- Create, read, update, delete events
- Event comments management
- Search events with filters

**Habits API:**
- Habit management (CRUD operations)
- Habit comments functionality
- Habit favorites

**User Management API:**
- Authentication (login/logout)
- Friend management
- User profile operations
- Own security settings

**Search API:**
- Search places
- Search events with parameters

### 📄 Page Objects Implemented

| Page | Purpose | Components Used | Test Coverage |
|------|---------|----------------|---------------|
| **HomePage** | Main landing page | HeaderComponent, FooterComponent | ✅ Smoke tests |
| **EventsPage** | Events listing | EventsListComponent, EventsFilterSectionComponent, EventsTopBarComponent | ✅ Search, Filter |
| **EventDetailsPage** | Single event view | EventDetailsCardComponent, CommentsComponent | ✅ Details, Comments |
| **CreateEventPage** | Create new events | EventsTopBarComponent, TagsComponent, ImageUploadComponent | ✅ Creation flow |
| **CreateUpdateEventPage** | Create/Edit events | Form components, DateTimePickerComponent | ✅ CRUD operations |
| **NewsPage** | News listing | NewsListComponent, NewsFilterSelectionComponent, NewsTopBarComponent | ✅ Browsing, Filter |
| **NewsDetailsPage** | News article view | NewsDetailsInfoComponent, CommentsComponent | ✅ Article details |
| **CreateNewsPage** | Create news articles | NewsFormComponent, NewsTagsComponent, NewsImageUploadComponent, NewsRichTextEditorComponent | ✅ Validation tests |
| **MySpacePage** | User dashboard | MyEventsTabComponent, MyNewsTabComponent, MyHabitsTabComponent, BookmarkTabComponent | ✅ User content |
| **ProfileEditPage** | Edit user profile | ProfileDetailsComponent, UploadAvatarComponent, PrivacySettingsComponent, EmailNotificationsComponent, SocialLinksComponent | ✅ Profile editing |
| **SavedPage** | Saved/bookmarked items | BookmarkTabComponent | Implemented |
| **CreateHabitPage** | Create eco-habits | HabitCardComponent, Form components | Implemented |

### 🧩 Component Library

**Authentication Components:**
- `SignInModalComponent` - User login dialog
- `SignUpModalComponent` - User registration dialog

**Event Components:**
- `EventCardComponent` - Individual event card
- `EventsListComponent` - Events list container
- `EventDetailsCardComponent` - Detailed event view
- `EventsFilterSectionComponent` - Event filtering UI
- `EventsTopBarComponent` - Events page top bar
- `MyEventsTabComponent` - User's events tab

**News Components:**
- `NewsCardComponent` - Individual news card
- `NewsListComponent` - News list container
- `NewsFormComponent` - News creation form
- `NewsDetailsInfoComponent` - News details display
- `NewsInfoComponent` - News information section
- `NewsFilterSelectionComponent` - News filters
- `NewsTopBarComponent` - News page top bar
- `NewsTagsComponent` - News tags selector
- `NewsImageUploadComponent` - News image uploader
- `NewsRichTextEditorComponent` - Rich text editor for news

**Profile & User Components:**
- `ProfileDetailsComponent` - User profile details
- `EditProfileButtonComponent` - Profile edit trigger
- `UploadAvatarComponent` - Avatar upload widget
- `PrivacySettingsComponent` - Privacy settings panel
- `EmailNotificationsComponent` - Email preferences
- `SocialLinksComponent` - Social media links
- `MyNewsTabComponent` - User's news tab
- `MyHabitsTabComponent` - User's habits tab

**Habit Components:**
- `HabitCardComponent` - Habit card display

**Common/Shared Components:**
- `HeaderComponent` - Site-wide header
- `FooterComponent` - Site-wide footer
- `CommentComponent` - Individual comment
- `CommentsComponent` - Comments list
- `BookmarkTabComponent` - Bookmarks/saved items
- `TagsComponent` - Generic tags selector
- `ImageUploadComponent` - Generic image uploader
- `ConfirmationPopUpComponent` - Confirmation dialogs
- `DateTimePickerComponent` - Date/time selector
- `DropDownComponent` - Dropdown selector
- `RichTextEditorComponent` - Generic rich text editor
- `MaterialCheckboxComponent` - Material design checkbox

**Modal Components:**
- `UploadImageModal` - Image upload dialog
- `DeleteCommentModal` - Delete confirmation modal
- `CancelJoiningEventModal` - Cancel event participation

## 🚀 Getting Started

### Prerequisites
- **.NET 8 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Visual Studio 2022/2026** or **VS Code**
- **Git** for version control
- **Chrome/Firefox/Edge** browser

### Installation

1. **Clone the repository:**
```bash
git clone https://github.com/UA-5340-TAQC/green_city_sh.git
cd green_city_sh
```

2. **Restore NuGet packages:**
```bash
dotnet restore
```

3. **Build the solution:**
```bash
dotnet build
```

4. **Verify tests are discovered:**
```bash
dotnet test --list-tests
```

### Running Tests

**Run all tests:**
```bash
dotnet test
```

**Run with detailed output:**
```bash
dotnet test --logger "console;verbosity=detailed"
```

**Run tests by category:**
```bash
dotnet test --filter "Category=Smoke"
dotnet test --filter "Category=Regression"
```

**Run specific test:**
```bash
dotnet test --filter "FullyQualifiedName~HomePageTests.VerifyHomePageLoads"
```

**In Visual Studio:**
1. Open **Test Explorer** (`Test → Test Explorer` or `Ctrl+E, T`)
2. Click **Run All** or select specific tests
3. View results, logs, and screenshots

## 📦 Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| **Selenium.WebDriver** | 4.43.0 | Core WebDriver API for browser automation |
| **Selenium.Support** | 4.43.0 | PageFactory and support utilities |
| **WebDriverManager** | 2.17.7 | Automatic browser driver download/management |
| **DotNetSeleniumExtras.WaitHelpers** | 3.11.0 | ExpectedConditions for explicit waits |
| **RestSharp** | 114.0.0 | REST API client library for API testing |
| **NUnit** | 3.14.0 | Unit testing framework |
| **NUnit3TestAdapter** | 4.5.0 | Visual Studio Test Explorer adapter |
| **NUnit.Analyzers** | 3.9.0 | Code analyzers for NUnit best practices |
| **Allure.Net.Commons** | 2.15.0 | Allure reporting core library |
| **Allure.NUnit** | 2.15.0 | Allure NUnit integration |
| **DotNetEnv** | 3.2.0 | .env file support for configuration |
| **Microsoft.NET.Test.Sdk** | 17.8.0 | Test platform SDK |

**All dependencies auto-restore during build.**

## 🔧 Configuration

### Default Settings

| Setting | Value | Configurable |
|---------|-------|-------------|
| **Base URL** | `https://www.greencity.cx.ua/#/greenCity` | Yes (Configuration.cs) |
| **Default Browser** | Chrome | Yes (BaseTest.cs) |
| **Wait Timeout** | 15 seconds | Yes (Configuration.cs) |
| **Page Load Timeout** | 30 seconds | Yes (.env) |
| **Implicit Wait** | 10 seconds | Yes (DriverFactory.cs) |
| **Headless Mode** | false | Yes (.env) |
| **Screenshots** | On test failure | Yes (BaseUITest.cs) |
| **Parallel Execution** | Enabled | Yes (BaseUITest.cs) |

### Environment Variables

The framework uses a `.env` file for configuration. Create `.env` in the project root (`green_city_sh.Tests/.env`):

```bash
# Application URLs
BASE_URL=https://www.greencity.cx.ua/#/greenCity
API_USER_BASE_URL=https://greencity-user.azurewebsites.net
API_GREENCITY_BASE_URL=https://greencity.azurewebsites.net

# Browser Configuration
BROWSER=Chrome
HEADLESS=false

# Timeouts (in seconds)
DEFAULT_TIMEOUT=15
PAGE_LOAD_TIMEOUT=30
API_TIME_OUT=30

# Test Credentials (Required for authentication tests)
TEST_EMAIL=your-test-email@example.com
TEST_PASSWORD=your-test-password
TEST_USER_ID=12345
TEST_USER_NAME=TestUser
```

**Required Variables:**
- `TEST_EMAIL` - Test account email
- `TEST_PASSWORD` - Test account password  
- `TEST_USER_ID` - Test user ID for API tests

**Optional Variables:**
- All others have defaults in `Configuration.cs`

### Configuration Class

```csharp
// Infrastructure/Configuration.cs
public static class Configuration
{
    public static string BaseUrl => 
        Environment.GetEnvironmentVariable("BASE_URL") 
        ?? "https://www.greencity.cx.ua/#/greenCity";

    public static int DefaultTimeout => 
        int.TryParse(Environment.GetEnvironmentVariable("DEFAULT_TIMEOUT"), out var timeout) 
        ? timeout 
        : 15;
}
```

### Browser Selection

```csharp
// In your test class
[SetUp]
public void Setup()
{
    // Use specific browser
    Driver = DriverFactory.CreateDriver(BrowserType.Chrome);
    // or BrowserType.Firefox
    // or BrowserType.Edge

    OnSetup();
}
```

## 📝 Writing Tests

### Test Structure Best Practices

1. **Inherit from BaseUITest or BaseAPITest** - Automatic setup/teardown with Allure
2. **Use Page Objects** - Never interact with WebDriver directly in UI tests
3. **Use API Clients** - Centralized REST API operations for API tests
4. **Compose with Components** - Leverage reusable components
5. **Add Categories** - Organize tests (Smoke, Regression, API, WEB, etc.)
6. **Descriptive Names** - Test names should describe behavior
7. **AAA Pattern** - Arrange, Act, Assert
7. **Use Assert.Multiple** - Group related assertions

### Example UI Test Class

```csharp
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;

namespace green_city_sh.Tests.Tests.WEB;

[TestFixture]
[Category("Smoke")]
public class HomePageTests : BaseUITest
{
    private HomePage? _homePage;

    protected override void OnSetup()
    {
        // Arrange - Initialize page and navigate
        _homePage = new HomePage(Driver!);
        _homePage.Open(BaseUrl);
    }

    [Test]
    [Category("Smoke")]
    public void VerifyHomePageLoads()
    {
        // Act
        var pageTitle = _homePage!.GetTitle();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(pageTitle, Is.Not.Empty, 
                "Page title should not be empty");
            Assert.That(pageTitle, Is.EqualTo("GreenCity — Build Eco-Friendly Habits Today"), 
                "Page title should match the expected value");
        });
    }
}
```

### Example API Test Class

```csharp
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Api.Clients.Greencity;
using green_city_sh.Tests.Api.DTO.EcoNews;

namespace green_city_sh.Tests.Tests.API;

[TestFixture]
[Category("API")]
public class EcoNewsApiTests : BaseAPITest
{
    private EcoNewsClient? _ecoNewsClient;

    [SetUp]
    public void SetUp()
    {
        _ecoNewsClient = new EcoNewsClient();
    }

    [Test]
    [Category("Smoke")]
    public async Task GetEcoNews_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await _ecoNewsClient!.GetEcoNewsPageAsync(0, 12);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(response.IsSuccessful, Is.True, "API call should succeed");
            Assert.That(response.Data, Is.Not.Null, "Response data should not be null");
            Assert.That(response.Data!.Page, Has.Count.GreaterThan(0), "Should return news items");
        });
    }
}
```

### Creating New Page Objects

```csharp
using OpenQA.Selenium;
using green_city_sh.Tests.Components;

namespace green_city_sh.Tests.Pages;

public class MyNewPage : BasePage
{
    // Lazy-loaded components
    private HeaderComponent? header;
    private FooterComponent? footer;

    // Public component properties
    public HeaderComponent Header => 
        header ??= new HeaderComponent(driver, By.TagName("header"));

    public FooterComponent Footer => 
        footer ??= new FooterComponent(driver, By.TagName("footer"));

    // Constructor
    public MyNewPage(IWebDriver driver) : base(driver)
    {
    }

    // Page-specific methods
    public void PerformAction()
    {
        // Implementation
    }
}
```

### Creating New Components

```csharp
using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class MyNewComponent : BaseComponent
{
    // Constructor accepting locator
    public MyNewComponent(IWebDriver driver, By rootLocator) 
        : base(driver, rootLocator)
    {
    }

    // Constructor accepting element
    public MyNewComponent(IWebDriver driver, IWebElement rootElement) 
        : base(driver, rootElement)
    {
    }

    // Component-specific methods
    public void ClickButton()
    {
        // Use RootElement for scoped searches
        var button = RootElement.FindElement(By.CssSelector("button"));
        button.Click();
    }
}
```

### Available Test Categories

Organize tests using NUnit categories:

**Test Layer Categories:**
- `[Category("API")]` - API/REST tests
- `[Category("WEB")]` or `[Category("UI")]` - Web UI tests

**Test Type Categories:**
- `[Category("Smoke")]` - Critical path tests
- `[Category("Regression")]` - Full regression suite
- `[Category("E2E")]` - End-to-end scenarios

**Feature Categories:**
- `[Category("Events")]` - Event-related features
- `[Category("News")]` - News-related features  
- `[Category("Profile")]` - User profile features
- `[Category("Authentication")]` - Login/Register tests
- `[Category("Habits")]` - Habit-related features
- `[Category("Comments")]` - Comment functionality
- `[Category("Search")]` - Search functionality

**Run by category:**
```bash
# Run all smoke tests
dotnet test --filter "Category=Smoke"

# Run all API tests
dotnet test --filter "Category=API"

# Run multiple categories
dotnet test --filter "Category=Events|Category=News"

# Run smoke tests for events
dotnet test --filter "Category=Smoke&Category=Events"
```

## 📸 Screenshots & Reporting

### Allure Reporting

The framework uses **Allure** for comprehensive test reporting:

- **Rich HTML reports** with test history and trends
- **Automatic screenshots** on test failure
- **Test execution timeline**
- **Categorization** by suites (API Tests, UI Tests)
- **GitHub Pages integration** for published reports
- **View latest report:** [Allure Report](https://ua-5340-taqc.github.io/green_city_sh/)

### Automatic Screenshot Capture

- **Triggered:** Automatically on test failure
- **Location:** `green_city_sh.Tests/bin/Debug/net8.0/Screenshots/`
- **Format:** `{TestName}_{Timestamp}.png`
- **Resolution:** Full browser window
- **Logged:** Path printed to test output
- **Allure Integration:** Screenshots attached to Allure report

### Implementation

```csharp
// In BaseUITest.cs
[TearDown]
[AllureStep("Tear Down Test Environment")]
public virtual void TearDown()
{
    try
    {
        if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
        {
            TakeScreenshot(TestContext.CurrentContext.Test.Name);
        }
        OnTearDown();
    }
    finally
    {
        Driver?.Quit();
        Driver?.Dispose();
    }
}
```

### Test Results

- **Test Explorer** - Visual Studio integration
- **Console Output** - Detailed logs with `--verbosity detailed`
- **NUnit XML** - Standard test results format (TRX files)
- **Allure Reports** - Rich HTML reports with history
- **CI/CD Integration** - GitHub Actions with artifact upload
- **GitHub Pages** - Published Allure reports

### Generate Allure Report Locally

```bash
# Run tests (generates allure-results)
dotnet test

# Generate and serve Allure report
allure serve allure-results
```

## 🧪 Test Coverage Summary

### Web UI Tests (21 Test Files)

| Test Suite | Test Count | Focus Area |
|------------|-----------|------------|
| **HomePageTests** | Multiple | Home page functionality |
| **EventsSearchTests** | Multiple | Event search features |
| **EventPageTests** | Multiple | Events listing |
| **EventDetailsPageTests** | Multiple | Event details view |
| **CreateEventsTests** | Multiple | Event creation |
| **CreateEventButtonTest** | Multiple | Create button behavior |
| **SearchTypeAndDateRangeTest** | Multiple | Advanced search |
| **TC037_EventCardUITest** | Multiple | Event card UI |
| **TC011_CreateEventTest** | Multiple | Event creation flow |
| **NewsPageTests** | Multiple | News listing |
| **NewsDetailsPageTests** | Multiple | News article view |
| **CreateNewsImageUploadTests** | Multiple | Image upload validation |
| **CreateNewsTitleValidationTests** | Multiple | Title validation |
| **CreateNewsSourceValidationTests** | Multiple | Source validation |
| **TC010_PublishButton_RemainsDisabled** | Multiple | Publish button state |
| **LoginTests** | Multiple | User authentication |
| **LogoutTests** | Multiple | User logout |
| **RegistrationTests** | Multiple | User registration |
| **InvalidPasswordTests** | Multiple | Password validation |
| **ProfileTests** | Multiple | Profile management |
| **TC001_AddCommentTests** | Multiple | Comment functionality |

### API Tests (10 Test Files)

| Test Suite | Test Count | Focus Area |
|------------|-----------|------------|
| **EcoNewsApiTests** | Multiple | EcoNews CRUD operations |
| **EcoNewsCommentApiTests** | Multiple | EcoNews comments API |
| **EventsAPITests** | Multiple | Events API endpoints |
| **EventCommentTests** | Multiple | Event comments API |
| **HabitApiTests** | Multiple | Habits CRUD operations |
| **HabitsCommentAPITests** | Multiple | Habit comments API |
| **FriendTests** | Multiple | Friends management API |
| **LoginTests (API)** | Multiple | Authentication API |
| **SearchEventsAPITests** | Multiple | Events search API |
| **SearchPlacesAPITests** | Multiple | Places search API |

**Total:** 31+ test suites with comprehensive coverage across UI and API layers

## 🤝 Contributing

### Code Standards

1. **Follow POM/COM/API Client** - Use design patterns consistently
2. **Inherit from Base Classes** - BasePage, BaseComponent, BaseModal, BaseApiClient
3. **No Hard-Coded Waits** - Use WebDriverWait, avoid Thread.Sleep
4. **Use Configuration** - No hard-coded URLs or timeouts
5. **Descriptive Names** - Clear, meaningful method and test names
6. **XML Documentation** - Document public methods
7. **Categories** - Tag tests with appropriate categories
8. **AAA Pattern** - Arrange-Act-Assert in tests
9. **Allure Attributes** - Use [AllureStep] for better reporting

### Naming Conventions

```csharp
// Pages: {Feature}Page
public class EventsPage : BasePage { }

// Components: {Feature}Component
public class EventCardComponent : BaseComponent { }

// Modals: {Feature}Modal
public class UploadImageModal : BaseModal { }

// API Clients: {Feature}Client
public class EcoNewsClient : BaseApiClient { }

// Tests: {Feature}Tests
public class EventsPageTests : BaseUITest { }
public class EcoNewsApiTests : BaseAPITest { }

// Methods: PascalCase, descriptive
public void ClickSubmitButton() { }
public bool IsErrorMessageDisplayed() { }
public async Task<RestResponse<EcoNewsModel>> GetEcoNewsAsync(int id) { }
```

### Pull Request Process

1. **Create Feature Branch**
```bash
git checkout -b feature/add-habits-api-tests
```

2. **Write Tests**
   - Add page objects, components, or API clients if needed
   - Create comprehensive tests following patterns
   - Add appropriate test categories

3. **Ensure Tests Pass**
```bash
dotnet test
```

4. **Code Formatting**
```bash
dotnet format
```

5. **Update Documentation**
   - Update README if adding major features
   - Add XML comments to public methods

6. **Commit with Clear Messages**
```bash
git commit -m "Add: Habits API CRUD tests with validation"
```

7. **Push and Create PR**
```bash
git push origin feature/add-habits-api-tests
```

### Code Review Checklist

- [ ] Tests are passing locally
- [ ] Code follows POM/COM/API Client patterns
- [ ] No hard-coded values (use Configuration)
- [ ] Proper exception handling
- [ ] XML documentation on public methods
- [ ] Meaningful test names and categories
- [ ] No Thread.Sleep (use explicit waits)
- [ ] Screenshots on failure working (UI tests)
- [ ] Allure attributes used appropriately
- [ ] No duplicate code
- [ ] Code formatted with `dotnet format`

## 🐛 Troubleshooting

### Common Issues & Solutions

| Issue | Cause | Solution |
|-------|-------|----------|
| **Driver not found** | WebDriverManager first run | Wait for automatic download |
| **Element not found** | Wrong locator or timing | Verify locator, add explicit wait |
| **Test timeout** | Slow page load | Increase wait timeout in Configuration |
| **Build fails** | Missing packages | Run `dotnet restore` |
| **Tests not discovered** | Build issue | Rebuild solution (`Ctrl+Shift+B`) |
| **Screenshot not saved** | Directory permissions | Check write permissions |
| **Browser doesn't start** | Driver mismatch | Clear driver cache, restart |

### Debug Tips

**1. Add explicit waits:**
```csharp
wait.Until(ExpectedConditions.ElementIsVisible(By.Id("element")));
```

**2. Inspect element in browser:**
- Run test with breakpoint
- Manually inspect element in DevTools
- Verify locator strategy

**3. Enable verbose logging:**
```bash
dotnet test --logger "console;verbosity=detailed"
```

**4. Check screenshot on failure:**
```
green_city_sh.Tests/bin/Debug/net8.0/Screenshots/
```

**5. Verify page load:**
```csharp
var url = _page.getUrl();
Console.WriteLine($"Current URL: {url}");
```

### Getting Help

- **Issues:** [GitHub Issues](https://github.com/UA-5340-TAQC/green_city_sh/issues)
- **Documentation:** See `green_city_sh.Tests/` folder docs
- **Selenium Docs:** [selenium.dev/documentation](https://www.selenium.dev/documentation/)
- **NUnit Docs:** [docs.nunit.org](https://docs.nunit.org/)

## 🚀 CI/CD Integration

### GitHub Actions Workflow

The project includes a comprehensive CI/CD pipeline (`.github/workflows/GreenCi.yml`):

**Features:**
- ✅ Automated build on push and pull requests
- ✅ Code formatting check with `dotnet format`
- ✅ Automated test execution
- ✅ Browser setup (Chrome)
- ✅ Environment variables configuration via GitHub Secrets
- ✅ Test results upload (TRX format)
- ✅ Allure report generation
- ✅ GitHub Pages deployment for Allure reports
- ✅ Parallel test execution

**Workflow Jobs:**
1. **Build Job** - Restore, build, lint code
2. **Tests Job** - Run all tests with configured environment
3. **Report Job** - Generate and publish Allure reports

**Required GitHub Secrets:**
```
BASE_URL
BROWSER
DEFAULT_TIMEOUT
HEADLESS
TEST_EMAIL
TEST_PASSWORD
TEST_USER_ID
TEST_USER_NAME
API_USER_BASE_URL
API_GREENCITY_BASE_URL
API_TIME_OUT
```

### View CI/CD Results

- **Actions Tab:** See all workflow runs
- **Pull Requests:** Automated checks on PRs
- **Allure Report:** [View Report](https://ua-5340-taqc.github.io/green_city_sh/)

### Manual GitHub Actions Workflow

You can also trigger the workflow manually from the Actions tab.

## 📊 Project Status

| Metric | Status | Details |
|--------|--------|---------|
| **Framework Status** | ✅ Production Ready | Fully functional with UI & API testing |
| **Build Status** | ✅ Passing | No compilation errors |
| **Test Coverage** | ✅ Comprehensive | 31+ test suites (21 UI + 10 API) |
| **Page Objects** | ✅ Implemented | 14 pages ready |
| **Components** | ✅ Implemented | 40 components ready |
| **Modals** | ✅ Implemented | 4 modal objects |
| **API Clients** | ✅ Implemented | 11 REST API clients |
| **DTOs** | ✅ Implemented | 20+ data models |
| **Documentation** | ✅ Complete | README + inline XML docs |
| **CI/CD** | ✅ Active | GitHub Actions with Allure reports |
| **Reporting** | ✅ Allure | Published to GitHub Pages |
| **Parallel Execution** | ✅ Enabled | Fast test execution |

## 👥 Team & Repository

**Team:** UA-5340-TAQC (Test Automation & Quality Control)  
**Repository:** [github.com/UA-5340-TAQC/green_city_sh](https://github.com/UA-5340-TAQC/green_city_sh)  
**Branch:** main  
**IDE:** Visual Studio Community 2026 (18.6.0)  
**Target Framework:** .NET 8.0  
**Test Framework:** NUnit 3.14.0  
**Reporting:** Allure 2.15.0

## 📚 Documentation & Resources

### Framework Documentation

| Document | Purpose | Location |
|----------|---------|----------|
| **README.md** | Main project documentation | Root folder |
| **Code Comments** | Inline XML documentation | Throughout codebase |
| **Allure Reports** | Test execution reports | [GitHub Pages](https://ua-5340-taqc.github.io/green_city_sh/) |
| **.env** | Environment configuration | green_city_sh.Tests/ |
| **allureConfig.json** | Allure configuration | green_city_sh.Tests/ |

### External Resources

- **[Selenium Documentation](https://www.selenium.dev/documentation/)** - WebDriver guide
- **[RestSharp Documentation](https://restsharp.dev/)** - REST API client
- **[NUnit Documentation](https://docs.nunit.org/)** - Testing framework
- **[Allure Documentation](https://docs.qameta.io/allure/)** - Test reporting
- **[Page Object Model](https://www.selenium.dev/documentation/test_practices/encouraged/page_object_models/)** - Design pattern
- **[C# .NET 8](https://learn.microsoft.com/en-us/dotnet/csharp/)** - Language reference
- **[WebDriver Wait](https://www.selenium.dev/documentation/webdriver/waits/)** - Explicit waits guide

## 🎯 Project Highlights

### ✨ Key Achievements

- ✅ **Comprehensive Test Coverage:** 31+ test suites covering UI and API layers
- ✅ **Modern Architecture:** Implements multiple design patterns (POM, COM, API Client, DTO)
- ✅ **Rich Component Library:** 40 reusable UI components for rapid test development
- ✅ **API Testing Layer:** 11 REST API clients with 20+ DTOs
- ✅ **CI/CD Pipeline:** Automated testing with GitHub Actions
- ✅ **Allure Reporting:** Beautiful test reports with history and trends
- ✅ **Parallel Execution:** Fast test runs with parallel execution support
- ✅ **Environment Configuration:** Flexible .env configuration system
- ✅ **Production Ready:** Stable, maintainable, and extensible framework

### 📈 Test Statistics

```
Total Test Suites: 31+
├── Web UI Tests: 21 suites
│   ├── Authentication: 4 suites
│   ├── Events: 7 suites
│   ├── News: 5 suites
│   └── Other: 5 suites
└── API Tests: 10 suites
    ├── EcoNews: 2 suites
    ├── Events: 2 suites
    ├── Habits: 2 suites
    ├── Friends: 1 suite
    ├── Search: 2 suites
    └── Authentication: 1 suite
```

## 📄 License

This project is part of the **Green City** ecological initiative.

---

## 🎓 Quick Start Guide

```bash
# 1. Clone repository
git clone https://github.com/UA-5340-TAQC/green_city_sh.git
cd green_city_sh

# 2. Create .env file in green_city_sh.Tests/
# Add your test credentials (see Configuration section)

# 3. Restore and build
dotnet restore
dotnet build

# 4. Run all tests
dotnet test

# 5. Run specific test category
dotnet test --filter "Category=Smoke"
dotnet test --filter "Category=API"
dotnet test --filter "Category=WEB"

# 6. View Allure report (if Allure CLI installed)
allure serve green_city_sh.Tests/bin/Debug/net8.0/allure-results

# 7. View results in Visual Studio
# Open Test Explorer (Ctrl+E, T)
```

---

<div align="center">

**Built with ❤️ for Green City**

🌿 **Automating Quality | Ensuring Sustainability** 🌿

### Current Status: Production Ready ✅

**Architecture:** Multi-layer (UI + API)  
**Framework Components:** 69+ Files (14 Pages + 40 Components + 4 Modals + 11 API Clients)  
**Test Suites:** 31+ (21 WEB + 10 API)  
**CI/CD:** GitHub Actions with Allure Reports  
**Build:** ✅ Passing | .NET 8.0

[📊 View Allure Report](https://ua-5340-taqc.github.io/green_city_sh/) | [🐛 Report Issues](https://github.com/UA-5340-TAQC/green_city_sh/issues) | [🤝 Contribute](https://github.com/UA-5340-TAQC/green_city_sh/pulls)

---

**Happy Testing!** 🧪

*Ensuring Green City's quality through comprehensive automated testing*

</div>
