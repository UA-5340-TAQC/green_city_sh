using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using green_city_sh.Tests.Api.Clients.Greencity;
using RestSharp;

namespace green_city_sh.Tests.Api.Clients.GreencityUser;

internal class FriendClient : BaseApiClient
{
    protected override string BaseUrl =>
        $"{base._baseUrl}/friends";

    public FriendClient(string baseUrl, string? authToken = null)
        : base(baseUrl, authToken)
    {
    }

    public RestResponse GetFriends(
        bool filterByCity = false,
        int page = 0,
        int size = 20)
    {
        var request = PrepareRequest("", Method.Get);

        request.AddQueryParameter("filterByCity", filterByCity);
        request.AddQueryParameter("page", page);
        request.AddQueryParameter("size", size);

        return Client.Execute(request);
    }
    public RestResponse AddFriend(long friendId)
    {
        var request = PrepareRequest($"/{friendId}", Method.Post);

        return Client.Execute(request);
    }
    public RestResponse DeleteFriend(long friendId)
    {
        var request = PrepareRequest($"/{friendId}", Method.Delete);

        return Client.Execute(request);
    }
    public RestResponse CancelFriendRequest(long friendId)
    {
        var request = PrepareRequest(
            $"/{friendId}/cancelRequest",
            Method.Delete);

        return Client.Execute(request);
    }
}


