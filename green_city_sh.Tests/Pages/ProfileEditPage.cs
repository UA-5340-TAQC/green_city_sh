using green_city_sh.Tests.Components;
using green_city_sh.Tests.Modals;
using OpenQA.Selenium;

namespace green_city_sh.Tests.Pages;

public class ProfileEditPage : BasePage

{
    public ProfileEditPage(IWebDriver driver) : base(driver)
    {
    }
    
    private EmailNotificationsComponent EmailNotifications =>
        new(driver,By.XPath("//div[contains(@class, 'email-preferences')]"));
    private EditProfileButtonComponent FormActionButtons => 
        new EditProfileButtonComponent(driver,By.XPath("//*[contains(@class, 'buttons')]"));
    private ProfileDetailsComponent ProfileDetails =>
        new(driver,By.XPath("//app-edit-profile//form"));
    private PrivacySettingsComponent ProfilePrivacy => 
        new PrivacySettingsComponent(driver, By.XPath("//div[@class='privacy-wrapper']"));
    private SocialLinksComponent SocialLinks => 
        new SocialLinksComponent(driver,By.XPath("//*[@formcontrolname='socialNetworks']/parent::div"));
    private UploadAvatarComponent UploadAvatar => 
        new UploadAvatarComponent(driver,By.XPath("//div[@class='profile-avatar-wrapper']"));
    private UploadImageModal UploadModal => 
        new UploadImageModal(driver,By.XPath("//div[@class='main-container']"));
    private By SuccessMessage =>
        By.CssSelector("mat-snack-bar-container.success-snackbar");
    
    public ProfileEditPage OpenProfileEditPage(int userId)
    {
        var currentUrl = driver.Url;
        var uri = new Uri(currentUrl);
        driver.Navigate().GoToUrl($"{uri.Scheme}://{uri.Host}/#/greenCity/profile/{userId}/edit");
        return new ProfileEditPage(driver);
    }


    public ProfileEditPage EnterName(string name)
    {
        ProfileDetails.EnterName(name);
        return this;
    }

    public ProfileEditPage EnterCity(string cityName)
    {
        ProfileDetails.EnterCityName(cityName);
        return this; 
    }

    public ProfileEditPage EnterCredo(string text)
    {
        ProfileDetails.EnterCredo(text);
        return this;
    }

    public ProfileEditPage EnterLink(string link)
    {
        SocialLinks.ClickAddSocialLinkButton();
        SocialLinks.EnterLink(link);
        SocialLinks.ClickAddLinkButton();
        return this; 
    }

    public ProfileEditPage SelectPrivacyType(string category, string value)
    {
        ProfilePrivacy.SelectPrivacy(category, value);
        return this; 
    }

    public ProfileEditPage ClickCheckBoxButton(string type, bool enabled)
    {
        EmailNotifications.ToggleNotificationFrequency(type, enabled);
        return this; 
    }

    public ProfileEditPage SelectDropDownFrequency(string type, string frequency)
    {
        EmailNotifications.SetNotificationFrequency(type, frequency);
        return this;
    }

    public MySpacePage SaveEditedProfile()
    {
        FormActionButtons.ClickSaveBtn();
        return new MySpacePage(driver);
    }

    public MySpacePage CancelEditedProfile()
    {
        FormActionButtons.ClickCancelBtn();
        return new MySpacePage(driver);
    }

    public ProfileEditPage UploadNewAvatar(string imagePath)
    {
        UploadAvatar.ClickEditImageBtn();
        UploadModal.ClickUploadButton();
        UploadModal.UploadImage(imagePath);
        UploadModal.ClickSaveImgButton();
        return this; 
    }

    public string GetName()
    {
        return ProfileDetails.GetName();
    }

    public string GetCityName()
    {
        return ProfileDetails.GetCityName();
    }

    public string GetCredo()
    {
        return ProfileDetails.GetCredo();
    }

    public string GetSelectedPrivacyValue(string category)
    {
        return ProfilePrivacy.GetSelectedPrivacyValue(category);
    }
    public bool IsSaveButtonEnabled()
    {
        return FormActionButtons.IsSaveBtnEnabled();
    }
    public ProfileEditPage ToggleAllNotifications(bool enabled)
    {
        var types = new[]
        {
        "Receive system notifications",
        "Receive notifications for likes",
        "Receive notifications for comments",
        "Receive notifications for invites",
        "Receive notifications for places"
    };

        foreach (var type in types)
        {
            EmailNotifications.ToggleNotificationFrequency(type, enabled);
        }

        return this;
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


