using RestSharp;

namespace green_city_sh.Tests.Api.Clients.Greencity;

public class SearchClient : BaseApiClient
{

    public SearchClient(string baseUrl) : base(baseUrl)
    {
    }

    public RestResponse SearchPlaces(
        string token,
        string searchQuery,
        int page = 0,
        int size = 20,
        bool isFavorite = false)
    {

        var request = PrepareRequest("/search/places", Method.Get);
        request.AddQueryParameter("searchQuery", searchQuery);
        request.AddParameter("page", page);
        request.AddParameter("size", size);
        request.AddParameter("isFavorite", isFavorite);
        return Client.Execute(request);
    }

    public RestResponse SearchPlacesWithoutAuth(string searchQuery)
    {
        var request = PrepareRequest("/search/places", Method.Get);
        request.AddQueryParameter("searchQuery", searchQuery);
        return Client.Execute(request);
    }
}
