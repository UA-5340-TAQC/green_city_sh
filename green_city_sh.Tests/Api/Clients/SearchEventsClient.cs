using green_city_sh.Tests.Api.Clients.GreencityUser;
using RestSharp;

namespace green_city_sh.Tests.Api.Clients;

public class SearchEventsClient
{
    private readonly RestClient _client;
    
    
    public SearchEventsClient(string baseUrl)
    {
        _client = new RestClient(baseUrl);
    }

    public RestResponse SearchEvents(
        string token,
        string searchQuery,
        int page = 0,
        int size = 20)
    {
        var request = new RestRequest("/search/events", Method.Get);

        request.AddHeader("Authorization", $"Bearer {token}");
        request.AddQueryParameter("searchQuery", searchQuery);
        request.AddQueryParameter("page", page);
        request.AddQueryParameter("size", size);

        return _client.Execute(request);
    }

    public RestResponse SearchEventsWithoutAuth(string searchQuery)
    {
        var request = new RestRequest("/search/events", Method.Get);
        request.AddQueryParameter("searchQuery", searchQuery);

        return _client.Execute(request);
    }
}
