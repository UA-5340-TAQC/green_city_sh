using System.Text.Json;
using green_city_sh.Tests.Api.DTO.Habits;
using RestSharp;

namespace green_city_sh.Tests.Api.Clients.Greencity;

public class HabitClient : BaseApiClient
{
    protected override string BaseUrl => $"{base._baseUrl}/habit";
    private const string UriId = "/{id}";

    public HabitClient(string baseUrl, string? authToken = null) : base(baseUrl, authToken)
    {
    }

    public RestResponse<HabitFavorites> GetFavoriteHabits(int page = 0, int size = 20, string? lang = null)
    {
        var request = PrepareRequest($"{BaseUrl}/favorites", Method.Get)
            .AddParameter("lang", lang)
            .AddParameter("page", page)
            .AddParameter("size", size);
        var response = Client.Execute<HabitFavorites>(request);
        return response;
    }

    public RestResponse AddHabitToFavorite(long id)
    {
        var request = PrepareRequest($"{BaseUrl}{UriId}/favorites", Method.Post)
            .AddUrlSegment("id", id);
        return Client.Execute(request);

    }

    public RestResponse RemoveHabitFromFavorite(long id)
    {
        var request = PrepareRequest($"{BaseUrl}{UriId}/favorites", Method.Delete)
            .AddUrlSegment("id", id);
        return Client.Execute(request);
    }

    public RestResponse<HabitFavorites> GetHabits(int page = 0, int size = 20, string? lang = null)
    {
        var request = PrepareRequest($"{BaseUrl}", Method.Get)
            .AddParameter("lang", lang)
            .AddParameter("page", page)
            .AddParameter("size", size);
        var response = Client.Execute<HabitFavorites>(request);
        return response;
    }

    public RestResponse<HabitModel> CreateCustomHabit(HabitModel habit, string? imagePath = null)
    {
        var request = PrepareRequest($"{BaseUrl}/custom", Method.Post);

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        string json = JsonSerializer.Serialize(habit, options);
        request.AddFile("request", System.Text.Encoding.UTF8.GetBytes(json), "data.json", "application/json");

        if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
        {
            request.AddFile("image", File.ReadAllBytes(imagePath), "test.jpg", "image/jpeg");
        }
        else
        {
            request.AddFile("image", Array.Empty<byte>(), "empty.jpg", "image/jpeg");
        }

        return Client.Execute<HabitModel>(request);
    }

    public RestResponse DeleteHabit(long id)
    {
        var request = PrepareRequest($"{BaseUrl}/delete{UriId}", Method.Delete)
            .AddUrlSegment("id", id);
        return Client.Execute(request);
    }
}
