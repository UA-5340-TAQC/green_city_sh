using DotNetEnv;

namespace green_city_sh.Tests;

[SetUpFixture]
public class TestEnvironmentSetup
{
    [OneTimeSetUp]
    public void LoadEnvironmentVariables()
    {
        var envPath = Path.GetFullPath(
            Path.Combine(
                TestContext.CurrentContext.TestDirectory,
                "..", "..", "..",
                ".env"
            )
        );

        if (!File.Exists(envPath))
        {
            TestContext.WriteLine(
                $".env file not found at: {envPath}"
            );
        }

        Env.Load(envPath);

        TestContext.WriteLine(
            $"✅ TEST_EMAIL loaded = {Environment.GetEnvironmentVariable("TEST_EMAIL")}"
        );
    }
}
