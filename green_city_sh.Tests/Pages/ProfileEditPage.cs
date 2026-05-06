using green_city_sh.Tests.Components;
using green_city_sh.Tests.Modals;
using OpenQA.Selenium;
using Allure.NUnit.Attributes;

namespace green_city_sh.Tests.Pages;

public class ProfileEditPage : BasePage

{
    public ProfileEditPage(IWebDriver driver) : base(driver)
    {
    }

    private EmailNotificationsComponent EmailNotifications =>
        new(driver, By.XPath("//div[contains(@class, 'email-preferences')]"));
    private EditProfileButtonComponent FormActionButtons =>
        new EditProfileButtonComponent(driver, By.XPath("//*[contains(@class, 'buttons')]"));
    private ProfileDetailsComponent ProfileDetails =>
        new(driver, By.XPath("//app-edit-profile//form"));
    private PrivacySettingsComponent ProfilePrivacy =>
        new PrivacySettingsComponent(driver, By.XPath("//div[@class='privacy-wrapper']"));
    private SocialLinksComponent SocialLinks =>
        new SocialLinksComponent(driver, By.XPath("//*[@formcontrolname='socialNetworks']/parent::div"));
    private UploadAvatarComponent UploadAvatar =>
        new UploadAvatarComponent(driver, By.XPath("//div[@class='profile-avatar-wrapper']"));
    private UploadImageModal UploadModal =>
        new UploadImageModal(driver, By.XPath("//div[@class='main-container']"));
    private static readonly By SuccessMessage =
    By.CssSelector("mat-snack-bar-container .mat-mdc-snack-bar-label");


    [AllureStep("Open profile edit page for user with ID")]
    public void OpenProfileEditPage(int userId)
    {
        var currentUrl = driver.Url;
        var uri = new Uri(currentUrl);
        driver.Navigate().GoToUrl($"{uri.Scheme}://{uri.Host}/#/greenCity/profile/{userId}/edit");

    }

    [AllureStep("Enter name in profile edit page")]
    public ProfileEditPage EnterName(string name)
    {
        ProfileDetails.EnterName(name);
        return this;
    }

    [AllureStep("Enter city in profile edit page")]
    public ProfileEditPage EnterCity(string cityName)
    {
        ProfileDetails.EnterCityName(cityName);
        return this;
    }

    [AllureStep("Enter credo in profile edit page")]
    public ProfileEditPage EnterCredo(string text)
    {
        ProfileDetails.EnterCredo(text);
        return this;
    }

    [AllureStep("Enter link in profile edit page")]
    public ProfileEditPage EnterLink(string link)
    {
        SocialLinks.ClickAddSocialLinkButton();
        SocialLinks.EnterLink(link);
        SocialLinks.ClickAddLinkButton();
        return this;
    }

    [AllureStep("Select privacy type in profile edit page")]
    public ProfileEditPage SelectPrivacyType(string category, string value)
    {
        ProfilePrivacy.SelectPrivacy(category, value);
        return this;
    }

    [AllureStep("Click checkbox for email notifications in profile edit page")]
    public ProfileEditPage ClickCheckBoxButton(string type, bool enabled)
    {
        EmailNotifications.ToggleNotificationFrequency(type, enabled);
        return this;
    }

    [AllureStep("Select frequency for email notifications in profile edit page")]
    public ProfileEditPage SelectDropDownFrequency(string type, string frequency)
    {
        EmailNotifications.SetNotificationFrequency(type, frequency);
        return this;
    }

    [AllureStep("Save edited profile in profile edit page")]
    public MySpacePage SaveEditedProfile()
    {
        FormActionButtons.ClickSaveBtn();
        return new MySpacePage(driver);
    }

    [AllureStep("Cancel editing profile in profile edit page")]
    public MySpacePage CancelEditedProfile()
    {
        FormActionButtons.ClickCancelBtn();
        return new MySpacePage(driver);
    }

    [AllureStep("Upload new avatar in profile edit page")]
    public ProfileEditPage UploadNewAvatar(string imagePath)
    {
        UploadAvatar.ClickEditImageBtn();
        UploadModal.ClickUploadButton();
        UploadModal.UploadImage(imagePath);
        UploadModal.ClickSaveImgButton();
        return this;
    }

    [AllureStep("Check if Save button is enabled in profile edit page")]
    public bool IsSaveBtnEnabled()
    {
        return FormActionButtons.IsSaveBtnEnabled();
    }

    public ProfileEditPage WaitUntilLoaded()
    {
        wait.Until(_ => driver.FindElements(By.Id("name")).Any(e => e.Displayed));
        return this;
    }

    public IWebElement? GetSuccessMessage()
    {
        return wait.Until(driver =>
            driver.FindElements(SuccessMessage)
                .FirstOrDefault(e => e.Displayed));
    }
}
