namespace green_city_sh.Tests.Infrastructure;

public static class Configuration
{
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

    public static string TestEmail => Environment.GetEnvironmentVariable("TEST_EMAIL")
        ?? throw new InvalidOperationException("TEST_EMAIL environment variable is missing!");
    
    public static string TestPassword => Environment.GetEnvironmentVariable("TEST_PASSWORD") 
        ?? throw new InvalidOperationException("TEST_PASSWORD environment variable is missing!");

    public static string SmokeSearchKeyword => Environment.GetEnvironmentVariable("SMOKE_SEARCH_KEYWORD")
        ?? throw new InvalidOperationException("SMOKE_SEARCH_KEYWORD environment variable is missing!");
}
