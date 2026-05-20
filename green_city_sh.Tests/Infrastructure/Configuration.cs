using DotNetEnv;

namespace green_city_sh.Tests.Infrastructure;

public static class Configuration
{
    static Configuration()
    {
        Env.TraversePath().Load();
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
            throw new InvalidOperationException($"{name} environment variable is missing or empty! Please ensure it is set in your environment.");
        }
        return value;
    }

    public static string TestEmail => GetRequiredEnv("TEST_EMAIL");
    public static string TestPassword => GetRequiredEnv("TEST_PASSWORD");
    public static int TestUserId
    {
        get
        {
            var raw = Environment.GetEnvironmentVariable("TEST_USER_ID");
            if (!int.TryParse(raw, out var id) || id <= 0)
            {
                throw new InvalidOperationException(
                    "TEST_USER_ID must be set to a positive integer.");
            }
            return id;
        }
    }

    public static string TestUserName => GetRequiredEnv("TEST_USER_NAME");

    public static string ApiUserBaseUrl => GetRequiredEnv("API_USER_BASE_URL");
    public static string ApiGreenCityBaseUrl => GetRequiredEnv("API_GREENCITY_BASE_URL");
    public static int ApiTimeoutSeconds => int.TryParse(Environment.GetEnvironmentVariable("API_TIME_OUT"), out var timeout)
        ? timeout
        : 10;
}

