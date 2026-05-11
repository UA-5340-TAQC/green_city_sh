using System.Text;
using System.Text.Json;
using green_city_sh.Tests.Api.DTO.Habits;
using RestSharp;

namespace green_city_sh.Tests.Api.Clients;

public class HabitCommentClient : BaseApiClient
{
    protected override string BaseUrl => $"{base._baseUrl}/habits";

    public HabitCommentClient(string baseUrl, string? authToken = null) : base(baseUrl, authToken)
    {
    }

    public RestResponse<HabitCommentPageResponse> GetActiveComments(int habitId, int page = 0, int size = 5)
    {
        var request = PrepareRequest($"{BaseUrl}/comments/active", Method.Get)
            .AddQueryParameter("habitId", habitId)
            .AddQueryParameter("page", page)
            .AddQueryParameter("size", size)
            .AddHeader("Accept", "application/json");

        return Client.Execute<HabitCommentPageResponse>(request);
    }

    public RestResponse<HabitCommentResponse> AddComment(int habitId, string text)
    {
        var request = PrepareRequest($"{BaseUrl}/{{habitId}}/comments", Method.Post)
            .AddUrlSegment("habitId", habitId)
            .AddHeader("Accept", "application/json");

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var requestBody = new { text = text };
        var json = JsonSerializer.Serialize(requestBody, options);

        request.AlwaysMultipartFormData = true;
        request.AddFile("request", Encoding.UTF8.GetBytes(json), "request.json", "application/json");

        return Client.Execute<HabitCommentResponse>(request);
    }

    public RestResponse DeleteComment(int commentId)
    {
        var request = PrepareRequest($"{BaseUrl}/comments/{{id}}", Method.Delete)
            .AddUrlSegment("id", commentId)
            .AddHeader("Accept", "*/*");

        return Client.Execute(request);
    }
}