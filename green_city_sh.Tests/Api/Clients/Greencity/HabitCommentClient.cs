using System.Text;
using System.Text.Json;
using green_city_sh.Tests.Api.DTO.Habits;
using RestSharp;

namespace green_city_sh.Tests.Api.Clients.Greencity;

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
            .AddQueryParameter("size", size);

        return Client.Execute<HabitCommentPageResponse>(request);
    }

    public RestResponse<HabitCommentResponse> AddComment(int habitId, string text)
    {
        var request = PrepareRequest($"{BaseUrl}/{{habitId}}/comments", Method.Post)
            .AddUrlSegment("habitId", habitId);
        Console.WriteLine(request);
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var requestBody = new { text = text };
        var json = JsonSerializer.Serialize(requestBody, options);

        request.AlwaysMultipartFormData = true;
        request.AddFile("request", Encoding.UTF8.GetBytes(json), "request.json", "application/json");
        Console.WriteLine($"[API Request] {Method.Post} {Client.BuildUri(request)}");
        Console.WriteLine($"[Request Body] {json}");
        //return Client.Execute<HabitCommentResponse>(request);
        var response = Client.Execute<HabitCommentResponse>(request);
        Console.WriteLine($"[API Response] Status: {response.StatusCode}");
        if (!string.IsNullOrEmpty(response.Content))
        {
            Console.WriteLine($"[Response Content] {response.Content}");
        }

        if (response.ErrorException != null)
        {
            Console.WriteLine($"[Error] {response.ErrorException.Message}");
        }

        return response;

    }

    public RestResponse DeleteComment(int commentId)
    {
        var request = PrepareRequest($"{BaseUrl}/comments/{{id}}", Method.Delete)
            .AddUrlSegment("id", commentId)
            .AddHeader("Accept", "*/*");

        return Client.Execute(request);
    }
}
