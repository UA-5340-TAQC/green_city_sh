using Allure.Net.Commons.Attributes;
using green_city_sh.Tests.Infrastructure;
using RestSharp;

namespace green_city_sh.Tests.Api.Clients
{
    public abstract class BaseApiClient
    {

        protected readonly string _baseUrl;
        protected virtual string BaseUrl => _baseUrl;
        protected readonly RestClient Client;
        protected string? AuthToken;
        protected BaseApiClient(string baseUrl, string? authToken = null)
        {
            _baseUrl = baseUrl;
            AuthToken = authToken;
            var options = new RestClientOptions(BaseUrl)
            {
                ThrowOnAnyError = false,
                Timeout = TimeSpan.FromSeconds(Configuration.ApiTimeoutSeconds)
            };

            Client = new RestClient(options);
            if (!string.IsNullOrEmpty(AuthToken))
            {
                Client.AddDefaultHeader("Authorization", $"Bearer {AuthToken}");
            }
        }

        [AllureStep("Prepare API request: {method} {resource}")]
        protected RestRequest PrepareRequest(string resource, Method method, object? body = null, string acceptHeader = "application/json")
        {
            var request = new RestRequest(resource, method);
            if (body != null)
            {
                request.AddJsonBody(body);
            }
            request.AddHeader("Accept", acceptHeader);
            return request;
        }
    }
}
