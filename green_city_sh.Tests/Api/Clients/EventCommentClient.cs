using RestSharp;
using green_city_sh.Tests.Api.DTO;
using System.Text.Json;
using System.Text;

namespace green_city_sh.Tests.Api.Clients
{
    public class EventCommentClient : BaseApiClient
    {
        public EventCommentClient(string baseUrl, string? authToken = null)
            : base(baseUrl, authToken)
        {
        }

        public RestResponse GetComments(int eventId)
        {
            var request = PrepareRequest($"/events/{eventId}/comments", Method.Get);
            return Client.Execute(request);
        }

        public RestResponse AddComment(int eventId, AddCommentRequest payload)
        {
            var request = new RestRequest($"/events/{eventId}/comments", Method.Post);
            request.AddHeader("Accept", "application/json");

            request.AlwaysMultipartFormData = true;

            string jsonString = JsonSerializer.Serialize(payload);
            byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonString);
            request.AddFile("request", jsonBytes, "request.json", "application/json");

            return Client.Execute(request);
        }

        public RestResponse DeleteComment(int commentId)
        {
            var request = PrepareRequest($"/events/comments/{commentId}", Method.Delete);
            return Client.Execute(request);
        }
    }
}