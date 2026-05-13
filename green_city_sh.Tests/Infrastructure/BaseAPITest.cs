using OpenQA.Selenium;
using green_city_sh.Tests.Drivers;
using Allure.Net.Commons;
using Allure.Net.Commons.Attributes;

namespace green_city_sh.Tests.Infrastructure
{
    [TestFixture]
    [Allure.NUnit.AllureNUnit]
    [AllureSuite("API Tests")]
    public abstract class BaseAPITest
    {

    }
}
