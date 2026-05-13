using green_city_sh.Tests.Api.Clients.GreencityUser;
using RestSharp;

namespace green_city_sh.Tests.Api.Clients.Greencity;

public class SearchEventsClient : BaseApiClient
{
    private readonly RestClient _client;


    public SearchEventsClient(string baseUrl, string? token = null)
        : base(baseUrl, token) { }

    public RestResponse SearchEvents(
        string token,
        string searchQuery)
    {
        var request = new RestRequest("/search/events", Method.Get);

        request.AddHeader("Authorization", $"Bearer {token}");
        request.AddQueryParameter("searchQuery", searchQuery);
        request.AddQueryParameter("page", 0);
        request.AddQueryParameter("size", 20);

        return Client.Execute(request);
    }

    public RestResponse SearchEventsUnauthorized(string searchQuery)
    {
        var request = PrepareRequest("/search/events", Method.Get);
        request.AddQueryParameter("searchQuery", searchQuery);

        return Client.Execute(request);
    }
}
