using NUnit.Framework;
// using Allure.NUnit;
using Allure.NUnit.Attributes;

using SeverityLevel = Allure.Net.Commons.SeverityLevel;


[TestFixture]
[AllureSuite("Debug Allure")]
public class AllureSmokeTest
{
    [Test]
    // [AllureSeverity(SeverityLevel.normal)]
    public void Allure_Is_Working_Test()
    {
        Assert.Pass("Allure works");
    }
}