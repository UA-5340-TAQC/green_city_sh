using NUnit.Framework;
using DotNetEnv;

[SetUpFixture]
public class GlobalSetup
{
    [OneTimeSetUp]
    public void GlobalOneTimeSetUp()
    {
        // Traverses up the directory tree to find the .env file and loads environment variables into the process
        Env.TraversePath().Load();
    }
}