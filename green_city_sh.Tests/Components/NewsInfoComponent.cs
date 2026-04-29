using System.Text.RegularExpressions;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace green_city_sh.Tests.Components;

public class NewsInfoComponent : BaseComponent
{
    private By DateLabel => By.XPath(".//div[contains(@class, 'news-info-date')]");
    private By AuthorLabel => By.XPath(".//div[contains(@class, 'news-info-author')]");
    private By LikeBtn => By.XPath(".//div[contains(@class, 'like_wr')]"); 
    
    public NewsInfoComponent(IWebDriver driver, By rootLocator) : base(driver, rootLocator)
    {
    }

    public NewsInfoComponent(IWebDriver driver, IWebElement componentRoot) : base(driver, componentRoot)
    {
    }
    
    public string GetDateText()
    {
        WaitUntilElementVisibleBy(DateLabel);
        return FindElement(DateLabel).Text;
    }

    public string GetAuthorText() => 
        FindElement(AuthorLabel).Text;
    public void ClickLikeBtn() => 
        WaitAndClick(LikeBtn);
    
}