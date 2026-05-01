# 🌿 Green City Test Framework — Architectural & Code-Quality Audit (v2)

> **Audit date:** 2026-05-01 — re-run after `main` was updated with commits
> `86a7201` (fix time), `97b1ccd` (fix), `196e6ab` (update), `3cd4e5b` (fix).
> New test files added: `CreateEventButtonTest`, `LoginTests`, `RegistrationTests`,
> `InvalidPasswordTests`, `TC001_AddCommentTests`, `TC037_EventCardUITest`.
> New components: `DateTimePickerComponent`, `CommentComponent`, `NewsTagsComponent`,
> `NewsImageUploadComponent`. Updated pages: `MySpacePage`, `CreateUpdateEventPage`.

## Project Health Score: **5 / 10** — *Still fragile; three new issues introduced*

The framework backbone (`Base → BasePage/BaseComponent → POM`) remains sound.
`Configuration` centralises env-var reading and `BaseTest` owns the WebDriver lifecycle correctly.
However, the majority of issues identified in the first audit are unchanged, and three new
problems were introduced by the latest commits.

---

## 🔴 Critical

### C-1. Double `Driver.Quit()` / Double-Dispose — **UNCHANGED**

`HomePageTests` (line 33) and `TC011_CreateEventTest` (line 47) both declare their own
`[TearDown]` methods that call `Driver?.Quit()`. `BaseTest.TearDown()` *also* calls `Quit()`
and `Dispose()`, so the driver is torn down twice every test run, generating
`ObjectDisposedException` noise and masking real failures.

**Offending code:**
```csharp
// HomePageTests.cs:33
[TearDown]
public new void TearDown()    // NUnit sees TWO [TearDown] methods
{
    Driver?.Quit();            // BaseTest already does this!
}
```

**Fix:** Remove both overrides. Move any per-class cleanup into `OnTearDown()` and never call
`Driver.Quit()` there.

---

### C-2. `EventDetailsPage.RefreshPage()` Navigates Away Instead of Refreshing — **UNCHANGED**

`RefreshPage()` is a verbatim copy of `OpenViaMoreButton()`. It navigates back to `/events`
and opens the *first* available event — not the one that was open. `TC001_AddCommentTests`
calls this and silently tests a different event after "refresh".

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

### C-3. Dead `GC_TEST_EMAIL` / `GC_TEST_PASSWORD` Properties in `CreateEventsTests` — **UNCHANGED**

`CreateEventsTests` declares two private properties that read `GC_TEST_EMAIL` and
`GC_TEST_PASSWORD`, variables that are never set in CI or the `.env` template.
The actual `PerformLogin()` call correctly uses `Configuration.TestEmail`/`TestPassword`.
The two properties are dead code that will throw `InvalidOperationException` if ever referenced.

**Fix:** Delete the local `GetRequiredEnv` helper and the two unused properties.

---

### C-4. Redundant Local Credential Fields in `TC001_AddCommentTests` and `TC037_EventCardUITest` — **NEW**

Both files duplicate the credential-reading pattern that already exists in `Configuration`:

```csharp
// TC001_AddCommentTests.cs:12-15 — declared but NEVER used
private static string TestEmail = Environment.GetEnvironmentVariable("TEST_EMAIL")
    ?? throw new InvalidOperationException("TEST_EMAIL is not configured.");
private static string TestPassword = ...;
```

`TC037_EventCardUITest` re-reads them in `OnSetup()` into *instance* fields, then passes
those fields to `Login()` instead of using `Configuration.TestEmail`. If the pattern diverges,
tests silently use stale values.

**Fix:**
- In `TC001_AddCommentTests`: delete the two static fields (they are completely unused).
- In `TC037_EventCardUITest`: delete the instance fields; call `Configuration.TestEmail` /
  `Configuration.TestPassword` directly.

---

### C-5. `SMOKE_SEARCH_KEYWORD` Missing from CI Workflow — **NEW**

`Configuration.SmokeSearchKeyword` throws `InvalidOperationException` when
`SMOKE_SEARCH_KEYWORD` is not set. `EventsSearchTests.SearchByKeywordReturnsMatchingEvents`
calls it in the test body. The CI workflow (`GreenCi.yml`) provides `TEST_EMAIL`,
`TEST_PASSWORD`, `BASE_URL`, `BROWSER`, `DEFAULT_TIMEOUT`, `HEADLESS`, and `TEST_USER_ID`
— but **not** `SMOKE_SEARCH_KEYWORD`. Every run of this test in CI throws before any
assertion executes.

**Fix:** Add `SMOKE_SEARCH_KEYWORD: ${{ secrets.SMOKE_SEARCH_KEYWORD }}` to the `env:` block
of the `tests` job in `.github/workflows/GreenCi.yml`, and add the corresponding repository
secret.

---

### C-6. `CreateEventsTests.LocalTearDown()` Is an Empty Stub — Data Pollution — **UNCHANGED**

```csharp
[TearDown]
public void LocalTearDown()
{
    // Postconditions: Delete created event   <-- never implemented
}
```

Every run leaves orphaned events on the shared staging environment, contaminating results of
`EventsSearchTests` and `SearchTypeAndDateRangeTest`.

**Fix:** Store the created event's title in a field during `Act`, then navigate to the event
and delete it in `OnTearDown()`.

---

### C-7. Two Competing `[SetUpFixture]` Classes Load `.env` With Different Strategies — **UNCHANGED**

`GlobalSetup.cs` (root namespace) uses `Env.TraversePath().Load()` while
`TestEnvironmentSetup.cs` constructs an absolute path relative to `TestContext.CurrentContext.TestDirectory`.
NUnit runs both; the one that runs last wins, and the correct file is not guaranteed.

**Fix:** Remove `TestEnvironmentSetup`; keep `GlobalSetup` with `Env.TraversePath().Load()`.

---

## 🟡 Warning

### W-1. `EventDetailsPageTests` and `NewsDetailsPageTests` — Hardcoded DB IDs and Exact Counts — **UNCHANGED**

```csharp
Driver!.Navigate().GoToUrl(BaseUrl + "/events/42");    // EventDetailsPageTests
Driver!.Navigate().GoToUrl(BaseUrl + "/news/10326");   // NewsDetailsPageTests
Assert.That(newsDetailsPage.GetComments().Count, Is.EqualTo(7), ...); // brittle
```

Any DB reset or comment added by another tester breaks these tests.

**Fix:** Move IDs to `Configuration` / `TestData`. Replace equality assertions on counts with
`Is.GreaterThanOrEqualTo` where the precise count is not the behaviour under test.

---

### W-2. `SearchTypeAndDateRangeTest` — Assertion Outside Conditional Guard — **NEW**

```csharp
if (eventsPage.EventsTopBar.IsSearchIconEnabled())
{
    searchResults = eventsPage.EventList.GetAllEventCards();
}
Assert.That(searchResults.Count, Is.GreaterThan(0), ...); // runs even when list is empty!
```

If the search icon is not enabled, `searchResults` remains an empty `List<EventCardComponent>`
and the assertion fails for the wrong reason. The same empty-list check is duplicated inside
the `if` block on the next line.

**Fix:** Remove the conditional guard entirely (a search icon that is not enabled is itself a
test failure), or move the `Assert` inside the `if` block and add an explicit failure outside.

---

### W-3. Implicit + Explicit Wait Combination — Reliability Anti-Pattern — **UNCHANGED**

`DriverFactory` sets `ImplicitWait = DefaultTimeout` (default 15 s). Every page and component
also uses `WebDriverWait` for explicit waits. When an explicit wait polls for element
*absence* or a negative condition, the effective timeout becomes `implicit + explicit` seconds,
making failures take 30 s+ and producing misleading timing.

**Fix:** Set `ImplicitWait = TimeSpan.Zero` in `DriverFactory`; rely exclusively on explicit waits.

---

### W-4. `EventCardComponent` — Duplicate Locators and Duplicate Methods — **UNCHANGED**

| Duplicate pair | Lines |
|---|---|
| `EventImageLocator` vs `ImageLocator` | 13 & 16 — identical CSS selector |
| `GetDate()` vs `GetDateText()` | 60 & 150 — identical bodies |
| `GetTime()` vs `GetTimeText()` | 61 & 153 — identical bodies |
| `GetStatus()` vs `GetStatusTest()` (typo) | 70 & 156 — identical bodies |

**Fix:** Delete the duplicates. Standardise on `GetDate`, `GetTime`, `GetStatus`, and `EventImageLocator`.

---

### W-5. `BaseTest` Always Creates Chrome — `Configuration.Browser` Is Ignored — **UNCHANGED**

```csharp
Driver = DriverFactory.CreateDriver(BrowserType.Chrome); // hardcoded
```

`Configuration.Browser` and `DriverFactory`'s Firefox/Edge branches are dead code.

**Fix:** `Driver = DriverFactory.CreateDriver(Enum.Parse<BrowserType>(Configuration.Browser, true));`

---

### W-6. Component Scope Leakage — `driver.FindElements()` Inside Components — **UNCHANGED**

`CommentsComponent` (`GetCommentCount`, `GetFirstCommentText`, `GetFirstCommentAuthor`,
`IsCommentVisible`, `DeleteComment`) and `EventsListComponent` (`GetEventCardsCount`,
`IsEndPageMessageDisplayed`) call `driver.FindElements()` instead of `RootElement.FindElements()`.

If the same CSS selector appears outside the component's root, the methods return incorrect
results.

**Fix:** Replace `driver.FindElements(...)` with `RootElement.FindElements(...)` in all components.

---

### W-7. `NewsPageTests.OnSetup` — Hardcoded Production URL — **UNCHANGED**

```csharp
Driver.Navigate().GoToUrl("https://www.greencity.cx.ua/#/greenCity/news"); // hardcoded!
```

**Fix:** `Driver.Navigate().GoToUrl($"{Configuration.BaseUrl}/news");`

---

### W-8. `HeaderComponent.WaitForUserLoggedIn()` Allocates a Redundant `WebDriverWait` — **UNCHANGED**

```csharp
new WebDriverWait(driver, TimeSpan.FromSeconds(Configuration.DefaultTimeout))
    .Until(_ => IsUserLoggedIn()); // should reuse `this.wait`
```

**Fix:** `wait.Until(_ => IsUserLoggedIn());`

---

### W-9. `[Retry]` Applied to Stateful (Write-Path) Tests — **UNCHANGED**

`EventDetailsPageTests` uses `[Retry(2)]` on tests that save events, join events, and click
bookmarks. A mid-test failure leaves the DOM in a mutated state; the retry then finds an
unexpected state and either produces a false positive or a cascading failure.

**Fix:** Remove `[Retry]` from all tests that perform write operations.

---

### W-10. `NewsImageUploadComponent` — Four Stub Methods Always Return Hardcoded Values — **NEW**

```csharp
public bool IsCropperDisplayed()      => false;  // never queries the DOM
public bool IsImagePreviewDisplayed() => false;  // never queries the DOM
public void ClickCancel()             { }        // no-op
public void ClickSubmit()             { }        // no-op
```

Any test relying on these methods will receive incorrect results or silently do nothing.

**Fix:** Implement the four methods using the locators that are already declared in the class
(`CropperBlock`, `CropperArea`, `CancelButton`, `SubmitButton`).

---

### W-11. Allure Integration Is a Stub — **UNCHANGED**

`BaseTest` carries `[AllureNUnit]`, but no test method uses `[AllureFeature]`,
`[AllureSeverity]`, `[AllureStep]`, or `[AllureDescription]`. The report contains only raw
test names.

**Fix:** Annotate test methods with `[AllureSeverity]` and `[AllureFeature]`; add `[AllureStep]`
to key page/component actions.

---

### W-12. Mixed NUnit Assertion Styles — **UNCHANGED**

Legacy calls (`Assert.IsTrue`, `Assert.IsFalse`, `Assert.IsNotNull`) still appear in
`EventDetailsPageTests`, `LoginTests`, and `EventPageTests`.

**Fix:** Standardise on `Assert.That` throughout.

---

## 🔵 Optimization

### O-1. Two Page Classes for One Page — `CreateEventPage` vs `CreateUpdateEventPage` — **UNCHANGED**

Both model `/events/create-update-event` with overlapping locators. Any selector change needs
two edits.

**Fix:** Delete `CreateEventPage`; migrate `TC011_CreateEventTest` to use `CreateUpdateEventPage`.

---

### O-2. Login Boilerplate Repeated Across Test Classes — **UNCHANGED**

At least eight test classes (`EventDetailsPageTests`, `EventsSearchTests`, `SearchTypeAndDateRangeTest`,
`CreateEventButtonTest`, `TC001_AddCommentTests`, `TC037_EventCardUITest`, `CreateEventsTests`,
`TC011_CreateEventTest`) repeat the same 4-6 line login sequence.

**Fix:** Extract an `AuthenticatedBaseTest` intermediate class.

---

### O-3. `CreateEventPage` Methods Allocate Fresh `WebDriverWait` Instances — **NEW**

`SelectInvite()`, `ClickOnlineCheckbox()`, and `EnterOnlineLink()` each create a new
`WebDriverWait` instead of using the inherited `wait` field from `Base`:

```csharp
public CreateEventPage SelectInvite()
{
    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Configuration.DefaultTimeout)); // local!
    ...
}
```

**Fix:** Remove the local declarations and use `this.wait` directly.

---

### O-4. `DateTimePickerComponent.EnterDate` — Incorrect Wait Lambda — **NEW**

```csharp
var element = wait.Until(d => RootElement.FindElement(_inputLocator));
```

The lambda parameter `d` (the driver) is never used; `RootElement` is captured from the outer
scope. This pattern is misleading and can hide stale-element issues because `RootElement` is
resolved once during construction.

**Fix:**
```csharp
var element = wait.Until(_ => RootElement.FindElement(_inputLocator));
// or simply:
var element = FindElement(_inputLocator);
```

---

### O-5. `TC037_EventCardUITest` — Step Numbers Encoded in Method Names — **UNCHANGED**

NUnit tests are independent; `TC037_Step1_*`, `TC037_Step2_*` etc. imply a sequential
dependency that does not exist and breaks Allure categorisation.

**Fix:** Rename to describe behaviour: `EventCard_IsDisplayedOnEventsPage`,
`EventCard_ImageIsPresentAndLoaded`, etc.

---

### O-6. `DriverFactory` — Firefox/Edge Drivers Miss `WebDriverManager` Setup — **UNCHANGED**

Only `CreateChromeDriver()` calls `WebDriverManager`; Firefox and Edge will fail on CI unless
a browser binary is pre-installed.

**Fix:** Add `new DriverManager().SetUpDriver(new FirefoxConfig(), ...)` mirroring the Chrome branch.

---

### O-7. `WebElementExtensions.ClickWithRetry` Uses `Thread.Sleep` — **UNCHANGED**

```csharp
Thread.Sleep(500); // only Thread.Sleep in the entire codebase
```

**Fix:** Replace with a short `WebDriverWait` polling `ElementToBeClickable`.

---

## 📋 Prioritised Action Plan

| # | Priority | Task | File(s) | New? |
|---|----------|------|---------|------|
| 1 | **P0** | Fix double `Driver.Quit()` | `HomePageTests.cs`, `TC011_CreateEventTest.cs` | — |
| 2 | **P0** | Fix `EventDetailsPage.RefreshPage()` | `EventDetailsPage.cs` | — |
| 3 | **P0** | Remove dead `GC_TEST_EMAIL`/`GC_TEST_PASSWORD` + redundant local cred fields | `CreateEventsTests.cs`, `TC001_AddCommentTests.cs`, `TC037_EventCardUITest.cs` | TC001/TC037 **new** |
| 4 | **P0** | Add `SMOKE_SEARCH_KEYWORD` to CI workflow + repo secret | `.github/workflows/GreenCi.yml` | **new** |
| 5 | **P0** | Implement `LocalTearDown()` event cleanup | `CreateEventsTests.cs` | — |
| 6 | **P1** | Consolidate two `[SetUpFixture]` classes | `GlobalSetup.cs`, `TestEnvironmentSetup.cs` | — |
| 7 | **P1** | Remove `ImplicitWait`; use only explicit waits | `DriverFactory.cs` | — |
| 8 | **P1** | Implement `NewsImageUploadComponent` stub methods | `NewsImageUploadComponent.cs` | **new** |
| 9 | **P1** | Move hardcoded IDs/counts to `Configuration`/`TestData` | `EventDetailsPageTests.cs`, `NewsDetailsPageTests.cs` | — |
| 10 | **P1** | Fix `SearchTypeAndDateRangeTest` assertion logic | `SearchTypeAndDateRangeTest.cs` | **new** |
| 11 | **P1** | Merge `CreateEventPage` + `CreateUpdateEventPage` | Both page files | — |
| 12 | **P2** | Extract `AuthenticatedBaseTest` | New file | — |
| 13 | **P2** | Fix component scope leakage (`driver` → `RootElement`) | `EventsListComponent.cs`, `CommentsComponent.cs` | — |
| 14 | **P2** | Respect `Configuration.Browser` in `BaseTest` | `BaseTest.cs` | — |
| 15 | **P2** | Deduplicate `EventCardComponent` locators & methods | `EventCardComponent.cs` | — |
| 16 | **P2** | Fix local `WebDriverWait` in `CreateEventPage` | `CreateEventPage.cs` | **new** |
| 17 | **P2** | Fix `DateTimePickerComponent.EnterDate` wait pattern | `DateTimePickerComponent.cs` | **new** |
| 18 | **P2** | Add `WebDriverManager` setup for Firefox/Edge | `DriverFactory.cs` | — |
| 19 | **P3** | Add Allure annotations across test/page files | All test files | — |
| 20 | **P3** | Standardise on `Assert.That` constraint model | All test files | — |
| 21 | **P3** | Remove `[Retry]` from write-path tests | `EventDetailsPageTests.cs`, `EventPageTests.cs` | — |
| 22 | **P3** | Replace `Thread.Sleep` with explicit wait | `WebElementExtensions.cs` | — |
| 23 | **P3** | Fix hardcoded URL in `NewsPageTests.OnSetup` | `NewsPageTests.cs` | — |
| 24 | **P3** | Replace redundant `WebDriverWait` in `WaitForUserLoggedIn` | `HeaderComponent.cs` | — |
| 25 | **P3** | Rename `TC037_Step*` methods to behaviour names | `TC037_EventCardUITest.cs` | — |
