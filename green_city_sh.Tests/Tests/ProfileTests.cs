using green_city_sh.Tests.Components;
using green_city_sh.Tests.Infrastructure;
using green_city_sh.Tests.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace green_city_sh.Tests.Tests;

[TestFixture]
public class ProfileTests : BaseTest
{
    private HomePage? _homePage;

    protected override void OnSetup()
    {
        _homePage = new HomePage(Driver!);
        _homePage.Open(BaseUrl);
        _homePage.Header.ChangeLanguage("EN");
    }



    [Test]
    [Category("Profile")]
    public void EditProfile_ShouldSaveAndRevertChanges()
    {

        const string updatedName = "TestA";
        const string updatedCity = "Kyiv";
        const string updatedCredo = "Test credo!";


        const string originalName = "Test";
        const string originalCity = "Kharkiv";
        const string originalCredo = "Test credo";


        var wait = new WebDriverWait(Driver!, TimeSpan.FromSeconds(Configuration.DefaultTimeout));

        _homePage!.Header.ClickSignIn();

        var signInModal = SignInModalComponent.WaitAndCreate(Driver!);
        signInModal.Login(Configuration.TestEmail, Configuration.TestPassword);

        Driver!.Navigate().GoToUrl($"{BaseUrl}/profile");

        wait.Until(driver => driver.Url.Contains("/profile"));

        var editPage = new ProfileEditPage(Driver!)
            .OpenProfileEditPage(Configuration.TestUserId)
            .WaitUntilLoaded();

        editPage.EnterName(updatedName);
        editPage.EnterCity(updatedCity);
        editPage.EnterCredo(updatedCredo);
        editPage.SelectPrivacyType("Show my location", "Show everyone");
        editPage.SelectPrivacyType("Show my eco places", "Show everyone");
        editPage.SelectPrivacyType("Show my To-do list", "Show everyone");
        editPage.ToggleAllNotifications(false);

        Assert.That(editPage.IsSaveButtonEnabled(), Is.True,
            "Save button should be enabled after profile changes.");

        editPage.SaveEditedProfile();
        TestContext.WriteLine("Updated profile saved");

        editPage = new ProfileEditPage(Driver!)
            .OpenProfileEditPage(Configuration.TestUserId)
            .WaitUntilLoaded();


        editPage.EnterName(originalName);
        editPage.EnterCity(originalCity);
        editPage.EnterCredo(originalCredo);
        editPage.SelectPrivacyType("Show my location", "Show only me");
        editPage.SelectPrivacyType("Show my eco places", "Show only me");
        editPage.SelectPrivacyType("Show my To-do list", "Show only me");
        editPage.ToggleAllNotifications(true);

        Assert.That(editPage.IsSaveButtonEnabled(), Is.True,
            "Save button should be enabled after reverting profile changes.");

        TestContext.WriteLine("Before final Save");

        editPage.SaveEditedProfile();

        TestContext.WriteLine("Clicked final Save");

        var successMessage = editPage.GetSuccessMessage();

        Assert.That(successMessage, Is.Not.Null,
            "Success message should be displayed.");

    }

}
