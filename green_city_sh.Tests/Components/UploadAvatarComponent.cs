using OpenQA.Selenium;

namespace green_city_sh.Tests.Components;

public class UploadAvatarComponent : BaseComponent
{
    private By EditImageBtn => By.CssSelector(".details-img button");
    private By ProfileAvatarImage => By.CssSelector(".profile-avatar");

    public UploadAvatarComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public UploadAvatarComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }

    public void ClickEditImageBtn() =>
        FindElement(EditImageBtn).Click();

    public bool IsProfileAvatarDisplayed() =>
        FindElement(ProfileAvatarImage).Displayed;

    public bool IsProfileEditBtnEnabled() =>
        FindElement(EditImageBtn).Enabled;
}