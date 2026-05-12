using System.Text.Json;
using green_city_sh.Tests.Api.DTO.EcoNewsComment;
using RestSharp;

namespace green_city_sh.Tests.Api.Clients;

public class EcoNewsCommentClient : BaseApiClient
{
    protected override string BaseUrl => $"{base._baseUrl}/eco-news";
    private const string UriId = "/{id}";

    public EcoNewsCommentClient(string baseUrl, string? authToken = null) : base(baseUrl, authToken)
    {
    }

    public RestResponse AddComment(long ecoNewsId, AddCommentRequest commentDto, string imagePath = null)
    {
        var request = PrepareRequest($"{ecoNewsId}/comments", Method.Post);
        
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(commentDto, options);
        
        request.AddFile("request", System.Text.Encoding.UTF8.GetBytes(json), "request.json", "application/json");

        if (imagePath != null)
        {
            request.AddFile("images", imagePath, "image/jpeg");
        }

        return Client.Execute(request);
    }

    public RestResponse GetCommentById(long id)
    {
        var request = PrepareRequest($"comments/{id}", Method.Get);
        return Client.Execute(request);
    }

    public RestResponse UpdateComment(long id, string newText)
    {
        var request = PrepareRequest($"comments?commentId={id}", Method.Patch);
        
        request.AddStringBody(newText, DataFormat.None);
        
        return Client.Execute(request);
    }

    public RestResponse DeleteComment(long id)
    {
        var request = PrepareRequest($"comments/{id}", Method.Delete);
        return Client.Execute(request);
    }
}