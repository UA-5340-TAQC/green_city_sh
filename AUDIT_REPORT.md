# 🌿 Green City Test Framework — Architectural & Code-Quality Audit

## Project Health Score: **5.5 / 10** — *Functional but structurally fragile*

The framework has a solid conceptual skeleton: `Base → BasePage/BaseComponent → concrete POM classes`
is the right pattern, `Configuration` cleanly centralises env-var reading, and `BaseTest` correctly
owns the WebDriver lifecycle. However, a significant accumulation of critical bugs, anti-patterns,
and incomplete implementations prevents this from being considered production-quality automation.

---

## 🔴 Critical

### 1. Double `Driver.Quit()` / Double-Dispose

**`HomePageTests`** and **`TC011_CreateEventTest`** both declare their own `[TearDown]` methods that
call `Driver?.Quit()`. Because `BaseTest.TearDown()` *also* calls `Quit()` and `Dispose()`, the
driver is torn down twice per test run, which produces `ObjectDisposedException` noise and obscures
real failures.

**Offending code (`HomePageTests.cs`, `TC011_CreateEventTest.cs`):**
```csharp
[TearDown]
public void TearDown()       // NUnit sees TWO [TearDown] methods
{
    Driver?.Quit();          // BaseTest already does this — double-dispose!
}
```

**Fix:** Remove all `[TearDown]` overrides from derived test classes.  
Move any cleanup logic into `OnTearDown()` instead:
```csharp
protected override void OnTearDown()
{
    // specific cleanup only; NEVER call Driver.Quit() here
}
```

---

### 2. `EventDetailsPage.RefreshPage()` Does Not Actually Refresh the Page

`RefreshPage()` navigates back to `/events` and clicks the first available "More" button —
identical behaviour to `OpenViaMoreButton()`. Any test calling `RefreshPage()` silently tests
the **wrong** event.

**Offending code (`EventDetailsPage.cs`):**
```csharp
public EventDetailsPage RefreshPage()
{
    driver.Navigate().GoToUrl($"{Configuration.BaseUrl}/events"); // navigates away!
    // ... then clicks "More" on the FIRST event, not the previously opened one
}
```

**Fix:**
```csharp
public EventDetailsPage RefreshPage()
{
    driver.Navigate().Refresh();
    WaitUntilPageLoads();
    return this;
}
```

---

### 3. Credential Env-Var Inconsistency in `CreateEventsTests`

`CreateEventsTests` defines its own `GetRequiredEnv()` helper and declares `TestEmail`/`TestPassword`
properties reading **`GC_TEST_EMAIL`** / **`GC_TEST_PASSWORD`** — variables never defined in CI or
the `.env` template. The actual `PerformLogin()` call uses `Configuration.TestEmail` (`TEST_EMAIL`)
instead. The private properties are dead code that will throw on any machine without the `GC_*` vars.

**Offending code (`CreateEventsTests.cs`):**
```csharp
private static string TestEmail => GetRequiredEnv("GC_TEST_EMAIL");     // always throws in CI
private static string TestPassword => GetRequiredEnv("GC_TEST_PASSWORD");

private void PerformLogin(CreateUpdateEventPage page)
{
    signInModal.Login(Configuration.TestEmail, Configuration.TestPassword); // uses different vars
}
```

**Fix:** Delete the local `GetRequiredEnv` helper and the duplicate properties;  
use `Configuration.TestEmail` / `Configuration.TestPassword` everywhere.

---

### 4. Hardcoded, Environment-Specific IDs in Tests

`EventDetailsPageTests` and `NewsDetailsPageTests` hardcode database record IDs and assert
specific comment counts tied to a specific state of the staging database.

**Offending code:**
```csharp
Driver!.Navigate().GoToUrl(BaseUrl + "/events/42");       // EventDetailsPageTests
Driver!.Navigate().GoToUrl(BaseUrl + "/news/10326");      // NewsDetailsPageTests
Assert.That(newsDetailsPage.GetComments().Count, Is.EqualTo(7), ...); // breaks if anyone adds a comment
```

**Fix:**  
- Move IDs to `Configuration` (env vars) or a dedicated `TestData` class.
- Replace exact-count assertions with `Is.GreaterThanOrEqualTo` where the count is not
  the actual behaviour under test.

---

### 5. `CreateEventsTests.LocalTearDown()` Does Nothing — Data Pollution

```csharp
[TearDown]
public void LocalTearDown()
{
    // Postconditions: Delete created event   <-- never implemented
}
```

Every test run leaves orphaned events on the shared staging environment. This contaminates
search results for `EventsSearchTests`, `SearchTypeAndDateRangeTest`, etc., causing
non-deterministic failures.

**Fix:** Implement cleanup — store the created event's title/ID in a field and delete it via
the UI or API in `OnTearDown()`.

---

### 6. Two Competing `[SetUpFixture]` Classes

`GlobalSetup.cs` (root namespace) and `TestEnvironmentSetup.cs` both call `Env.Load()`.
NUnit runs both, with different path-resolution strategies; it is unclear which one wins
if they discover different `.env` files.

**Fix:** Remove one `[SetUpFixture]` and standardise on a single `.env` loading strategy.
`GlobalSetup` using `Env.TraversePath().Load()` is the more robust of the two.

---

## 🟡 Warning

### 7. Mixing Implicit and Explicit Waits (Reliability Anti-Pattern)

`DriverFactory` sets `ImplicitWait` to `DefaultTimeout` seconds, while every page and component
also uses `WebDriverWait`. An explicit wait polling for element *absence* or a *condition* can
take up to `implicit + explicit` seconds to time out, making test execution unpredictably slow.

**Fix:** Set `ImplicitWait = TimeSpan.Zero` in `DriverFactory` and rely exclusively on explicit
`WebDriverWait`.

```csharp
// DriverFactory.cs — change this:
driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Configuration.DefaultTimeout);
// to:
driver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero;
```

---

### 8. `EventCardComponent` — Duplicate Locators and Duplicate Methods

```csharp
private By EventImageLocator => By.CssSelector("img.event-image"); // line 13
private By ImageLocator      => By.CssSelector("img.event-image"); // line 16 — exact duplicate

public string GetDate()     => RootElement.FindElement(DateLocator).Text.Trim();  // line 60
public string GetDateText() => RootElement.FindElement(DateLocator).Text.Trim();  // line 150 — identical body

public string GetTime()     => RootElement.FindElement(TimeLocator).Text.Trim();  // line 61
public string GetTimeText() => RootElement.FindElement(TimeLocator).Text.Trim();  // line 153 — identical body

public string GetStatus()     => RootElement.FindElement(StatusLabelLocator).Text.Trim(); // line 70
public string GetStatusTest() => RootElement.FindElement(StatusLabelLocator).Text.Trim(); // line 156 — typo + identical
```

**Fix:** Delete the duplicate members. Standardise on `GetDate`, `GetTime`, `GetStatus`,
and `GetImage`.

---

### 9. `BaseTest` Always Creates Chrome — `Configuration.Browser` Is Ignored

```csharp
// BaseTest.cs
Driver = DriverFactory.CreateDriver(BrowserType.Chrome); // hardcoded, ignores Configuration.Browser
```

`Configuration.Browser` and `DriverFactory`'s Firefox/Edge support are dead code.

**Fix:**
```csharp
var browserType = Enum.Parse<BrowserType>(Configuration.Browser, ignoreCase: true);
Driver = DriverFactory.CreateDriver(browserType);
```

---

### 10. Component Scope Leakage — `driver.FindElements()` Inside Components

`EventsListComponent`, `CommentsComponent`, and others use `driver.FindElements(...)` instead
of `RootElement.FindElements(...)`. If the same CSS class appears outside the component root,
the component picks up the wrong elements.

**Offending code:**
```csharp
// EventsListComponent.cs
public int GetEventCardsCount() => driver.FindElements(AllEventCards).Count; // leaks outside root

// CommentsComponent.cs
public int GetCommentCount() { var elements = driver.FindElements(CommentCountLocator); ... }
```

**Fix:** Replace `driver.FindElements(...)` → `RootElement.FindElements(...)` inside all components.

---

### 11. `NewsPageTests.OnSetup` — Hardcoded Production URL

```csharp
Driver.Navigate().GoToUrl("https://www.greencity.cx.ua/#/greenCity/news"); // hardcoded!
```

This ignores `Configuration.BaseUrl` and always targets production.

**Fix:**
```csharp
Driver.Navigate().GoToUrl($"{Configuration.BaseUrl}/news");
```

---

### 12. `HeaderComponent.WaitForUserLoggedIn()` Creates a Redundant `WebDriverWait`

```csharp
public void WaitForUserLoggedIn()
{
    new WebDriverWait(driver, TimeSpan.FromSeconds(Configuration.DefaultTimeout))
        .Until(_ => IsUserLoggedIn()); // new allocation instead of reusing `this.wait`
}
```

**Fix:** `wait.Until(_ => IsUserLoggedIn());`

---

### 13. `[Retry]` Applied to Stateful Tests

Tests in `EventDetailsPageTests` and `EventPageTests` carry `[Retry(2)]` but perform write
operations (save event, join event, click bookmark). If a test fails mid-way and retries, the
DOM state is already mutated, producing false positives or cascading failures.

**Fix:** Remove `[Retry]` from write-path tests, or make them idempotent before adding it back.

---

### 14. Allure Integration Is Mostly a Stub

`BaseTest` carries `[AllureNUnit]`, but no test method uses `[AllureFeature]`,
`[AllureSuite]`, `[AllureSeverity]`, `[AllureStep]`, or `[AllureDescription]`.
The Allure report is generated with only raw test names and no contextual metadata.

**Fix:** Annotate all test methods with `[AllureSeverity]` and `[AllureFeature]`;
wrap key actions with `[AllureStep]` in page/component methods.

---

### 15. Mixed NUnit Assertion Styles

The codebase uses both the legacy API (`Assert.IsTrue`, `Assert.IsFalse`, `Assert.IsNotNull`)
and the preferred constraint model (`Assert.That(x, Is.True)`).

**Fix:** Standardise on `Assert.That` throughout for consistency and better failure messages.

---

## 🔵 Optimization

### 16. Two Page Classes for One Page: `CreateEventPage` vs `CreateUpdateEventPage`

Both classes model `/events/create-update-event` with overlapping locators (`TitleInput`,
`StartTimeInput`, `EndTimeInput`, …). Any locator change requires two updates.

**Fix:** Merge them into a single `CreateUpdateEventPage`, keeping the richer implementation.

---

### 17. Login Boilerplate Repeated in Every `OnSetup()`

Ten test classes repeat the same 4–6 line login sequence:

```csharp
homePage.Header.ChangeLanguage("En");
homePage.Header.ClickSignIn();
SignInModalComponent.WaitAndCreate(Driver!).Login(Configuration.TestEmail, Configuration.TestPassword);
```

**Fix:** Extract an `AuthenticatedBaseTest` intermediate class that performs login in `OnSetup()`,
and have all login-required test classes extend it.

```csharp
public abstract class AuthenticatedBaseTest : BaseTest
{
    protected override void OnSetup()
    {
        NavigateToBaseUrl();
        new HomePage(Driver!).Header
            .ChangeLanguage("En")
            .ClickSignIn();
        SignInModalComponent.WaitAndCreate(Driver!).Login(
            Configuration.TestEmail, Configuration.TestPassword);
    }
}
```

---

### 18. `TC037_EventCardUITest` — Step Numbers Encoded in Method Names

```csharp
public void TC037_Step1_FirstEventCard_IsVisible() { ... }
public void TC037_Step2_EventImage_IsPresentAndLoaded() { ... }
```

NUnit tests are independent; encoding step order in names implies a sequential dependency
that does not exist. Rename to describe *behaviour*, not step order.

---

### 19. `DriverFactory` — No `WebDriverManager` for Firefox/Edge

`CreateFirefoxDriver()` / `CreateEdgeDriver()` instantiate drivers without calling
`WebDriverManager`, so they fail on any machine without a pre-installed driver binary.

**Fix:** Mirror the Chrome pattern:
```csharp
new DriverManager().SetUpDriver(new FirefoxConfig(), VersionResolveStrategy.MatchingBrowser);
```

---

### 20. `WebElementExtensions.ClickWithRetry` Uses `Thread.Sleep`

```csharp
Thread.Sleep(500); // only Thread.Sleep in the entire codebase
```

**Fix:** Replace with a `WebDriverWait` polling `ElementToBeClickable`.

---

## 📋 Prioritised Action Plan

| # | Priority | Task | File(s) | Impact |
|---|----------|------|---------|--------|
| 1 | **P0** | Fix double `Driver.Quit()` | `HomePageTests.cs`, `TC011_CreateEventTest.cs` | Prevents driver crash noise |
| 2 | **P0** | Fix `EventDetailsPage.RefreshPage()` | `EventDetailsPage.cs` | Correctness |
| 3 | **P0** | Remove dead `GC_TEST_EMAIL`/`GC_TEST_PASSWORD` code | `CreateEventsTests.cs` | Prevents CI failures |
| 4 | **P0** | Implement `LocalTearDown()` to delete created events | `CreateEventsTests.cs` | Data isolation |
| 5 | **P1** | Consolidate two `[SetUpFixture]` classes into one | `GlobalSetup.cs`, `TestEnvironmentSetup.cs` | Eliminates double `.env` load |
| 6 | **P1** | Remove `ImplicitWait`; use only explicit waits | `DriverFactory.cs` | Eliminates timing ambiguity |
| 7 | **P1** | Extract `AuthenticatedBaseTest` | New file | Eliminates 10× login duplication |
| 8 | **P1** | Move hardcoded IDs/counts to `Configuration` / `TestData` | `EventDetailsPageTests.cs`, `NewsDetailsPageTests.cs` | Test stability |
| 9 | **P1** | Merge `CreateEventPage` + `CreateUpdateEventPage` | Both page files | DRY |
| 10 | **P2** | Fix component scope leakage (`driver` → `RootElement`) | `EventsListComponent.cs`, `CommentsComponent.cs` | Reliability |
| 11 | **P2** | Respect `Configuration.Browser` in `BaseTest` | `BaseTest.cs` | Cross-browser coverage |
| 12 | **P2** | Deduplicate `EventCardComponent` locators & methods | `EventCardComponent.cs` | Cleanliness |
| 13 | **P2** | Add `WebDriverManager` setup to Firefox/Edge | `DriverFactory.cs` | CI portability |
| 14 | **P3** | Add `[AllureFeature]`, `[AllureSeverity]`, `[AllureStep]` | All test/page files | Reporting quality |
| 15 | **P3** | Standardise on `Assert.That` constraint model | All test files | Consistency |
| 16 | **P3** | Remove `[Retry]` from stateful tests | `EventDetailsPageTests.cs`, `EventPageTests.cs` | Reliability |
| 17 | **P3** | Replace `Thread.Sleep` with explicit wait | `WebElementExtensions.cs` | Performance |
| 18 | **P3** | Rename `TC037_Step*` methods to behaviour names | `TC037_EventCardUITest.cs` | Readability |
| 19 | **P3** | Fix hardcoded URL in `NewsPageTests.OnSetup` | `NewsPageTests.cs` | Environment portability |
| 20 | **P3** | Replace redundant `WebDriverWait` in `WaitForUserLoggedIn` | `HeaderComponent.cs` | Minor perf/cleanliness |
