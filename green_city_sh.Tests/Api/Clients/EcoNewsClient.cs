using System.Text.Json;
using green_city_sh.Tests.Api.DTO.EcoNews;
using RestSharp;

namespace green_city_sh.Tests.Api.Clients;

public class EcoNewsClient : BaseApiClient
{
    protected override string BaseUrl => $"{base._baseUrl}/eco-news";
    private const string UriId = "/{id}";

    public EcoNewsClient(string baseUrl, string? authToken = null) : base(baseUrl, authToken)
    {
    }

    public RestResponse<EcoNewsModel> GetEcoNewsById(long id, string lang = "en")
    {
        var request = PrepareRequest($"{BaseUrl}{UriId}", Method.Get)
            .AddQueryParameter("lang", lang)
            .AddUrlSegment("id", id)
            .AddHeader("Accept", "application/json");
        return Client.Execute<EcoNewsModel>(request);
    }

    public RestResponse<EcoNewsPageResponse> GetAllEcoNews(string? lang = null)
    {
        var request = PrepareRequest(BaseUrl, Method.Get);
        return Client.Execute<EcoNewsPageResponse>(request);
    }

    public RestResponse<EcoNewsModel> CreateEcoNews(CreateEcoNewsRequest ecoNews, string? imagePath = null)
    {
        var request = PrepareRequest(BaseUrl, Method.Post)
            .AddHeader("Accept", "application/json");

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var json = JsonSerializer.Serialize(ecoNews, options);
        request.AddFile("addEcoNewsDtoRequest", System.Text.Encoding.UTF8.GetBytes(json), "blob", "application/json");
        AttachImageToEcoNews(request, imagePath);

        return Client.Execute<EcoNewsModel>(request);
    }

    public RestResponse<EcoNewsModel> UpdateEcoNewsById(UpdateEcoNewsRequest ecoNews, long id, string? imagePath = null)
    {
        var request = PrepareRequest($"{BaseUrl}{UriId}", Method.Put)
            .AddUrlSegment("id", id);
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var json = JsonSerializer.Serialize(ecoNews, options);
        request.AddFile("updateEcoNewsDto", System.Text.Encoding.UTF8.GetBytes(json), "name", "application/json");
        AttachImageToEcoNews(request, imagePath);
        return Client.Execute<EcoNewsModel>(request);
    }

    public RestResponse DeleteEcoNewsById(long id, string lang = "en")
    {
        var request = PrepareRequest($"{BaseUrl}{UriId}", Method.Delete)
            .AddQueryParameter("lang", lang)
            .AddUrlSegment("id", id)
            .AddHeader("Accept", "application/json");
        return Client.Execute(request);
    }

    public RestResponse AddEcoNewsToFavorites(long id)
    {
        var request = PrepareRequest($"{BaseUrl}{UriId}/favorites", Method.Post)
            .AddUrlSegment("id", id)
            .AddHeader("Accept", "application/json");
        return Client.Execute(request);
    }

    public RestResponse DeleteEcoNewsFromFavorites(long id)
    {
        var request = PrepareRequest($"{BaseUrl}{UriId}/favorites", Method.Delete)
            .AddUrlSegment("id", id)
            .AddHeader("Accept", "application/json");
        return Client.Execute(request);
    }

    public void AttachImageToEcoNews(RestRequest request, string? imagePath)
    {
        if (imagePath == null)
        {
            request.AddFile("image", new byte[0], "empty.jpg", "image/jpeg");
            return;
        }

        var type = imagePath.EndsWith(".png") ? "image/png" : "image/jpeg";
        request.AddFile("image", imagePath, type);
    }
}