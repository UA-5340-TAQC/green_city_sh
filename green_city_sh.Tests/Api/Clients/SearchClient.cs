using RestSharp;

namespace green_city_sh.Tests.Api.Clients;

public class SearchClient
{
    private readonly RestClient _client;

    public SearchClient(string baseUrl)
    {
        _client = new RestClient(baseUrl);
    }

    public RestResponse SearchPlaces(
        string token,
        string searchQuery,
        int page = 0,
        int size = 20,
        bool isFavorite = false)
    {
        var request = new RestRequest("/search/places", Method.Get);
        request.AddHeader("Authorization", $"Bearer {token}");
        request.AddQueryParameter("searchQuery", searchQuery);
        request.AddParameter("page", page);
        request.AddParameter("size", size);
        request.AddParameter("isFavorite", isFavorite);
        return _client.Execute(request);
    }

    public RestResponse SearchPlacesWithoutAuth(string searchQuery)
    {
        var request = new RestRequest("/search/places", Method.Get);
        request.AddQueryParameter("searchQuery", searchQuery);
        return _client.Execute(request);
    }
}
