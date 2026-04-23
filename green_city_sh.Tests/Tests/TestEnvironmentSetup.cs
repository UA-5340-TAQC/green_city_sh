using DotNetEnv;
using NUnit.Framework;
using System.IO;

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
            throw new FileNotFoundException($".env file not found at: {envPath}");
        }

        Env.Load(envPath);

        TestContext.WriteLine(
            $"✅ TEST_EMAIL loaded = {Environment.GetEnvironmentVariable("TEST_EMAIL")}"
        );
    }
}