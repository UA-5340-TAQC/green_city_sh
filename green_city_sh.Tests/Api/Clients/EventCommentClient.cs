using RestSharp;
using green_city_sh.Tests.Api.DTO;

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
            var request = PrepareRequest($"/events/{eventId}/comments", Method.Post, payload);
            return Client.Execute(request);
        }

        public RestResponse DeleteComment(int commentId)
        {
            var request = PrepareRequest($"/events/comments/{commentId}", Method.Delete);
            return Client.Execute(request);
        }
    }
}