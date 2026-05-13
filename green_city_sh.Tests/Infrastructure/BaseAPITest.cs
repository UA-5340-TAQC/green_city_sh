using Allure.Net.Commons.Attributes;

namespace green_city_sh.Tests.Infrastructure
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    [Allure.NUnit.AllureNUnit]
    [AllureSuite("API Tests")]
    public abstract class BaseAPITest
    {

    }
}
