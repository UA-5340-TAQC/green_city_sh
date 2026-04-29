using DotNetEnv;

namespace green_city_sh.Tests.Infrastructure;

public static class Configuration
{
    static Configuration()
    {
        Env.Load();
    }
    public static string BaseUrl => Environment.GetEnvironmentVariable("BASE_URL") ?? "https://www.greencity.cx.ua/#/greenCity";

    public static string Browser => Environment.GetEnvironmentVariable("BROWSER") ?? "Chrome";

    public static int DefaultTimeout => int.TryParse(Environment.GetEnvironmentVariable("DEFAULT_TIMEOUT"), out var timeout) 
        ? timeout 
        : 15;
    public static int PageLoadTimeout => int.TryParse(Environment.GetEnvironmentVariable("PAGE_LOAD_TIMEOUT"), out var timeout) 
        ? timeout 
        : 30;

    public static bool HeadlessMode => bool.TryParse(Environment.GetEnvironmentVariable("HEADLESS"), out var headless) 
        && headless;

    private static string GetRequiredEnv(string name)
    {
        var value = Environment.GetEnvironmentVariable(name);
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"{name} environment variable is missing or empty! Please check your .runsettings file.");
        }
        return value;
    }

    public static string TestEmail => GetRequiredEnv("TEST_EMAIL");
    public static string TestPassword => GetRequiredEnv("TEST_PASSWORD");
    public static string SmokeSearchKeyword => GetRequiredEnv("SMOKE_SEARCH_KEYWORD");
}
