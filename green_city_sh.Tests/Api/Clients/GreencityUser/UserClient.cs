using green_city_sh.Tests.Api.DTO.User;
using RestSharp;

namespace green_city_sh.Tests.Api.Clients.GreencityUser;

public class UserClient : BaseApiClient
{
    protected override string BaseUrl => $"{base._baseUrl}/users";

    public UserClient(string baseUrl, string? authToken = null) : base(baseUrl, authToken)
    {
    }

    public RestResponse CreateUser(CreateUserRequestDto requestBody)
    {
        var request = PrepareRequest($"{BaseUrl}/create", Method.Post, requestBody);
        return Client.Execute(request);
    }

    public RestResponse GetExternalProfiles(string[] emails)
    {
        var request = PrepareRequest($"{BaseUrl}/profiles/external", Method.Get);
        foreach (var email in emails)
        {
            request.AddQueryParameter("emails", email);
        }
        return Client.Execute(request);
    }

    public RestResponse UpdateUserName(string email, string userName)
    {
        var request = PrepareRequest($"{BaseUrl}/user/name", Method.Patch);
        request.AddQueryParameter("email", email);
        request.AddQueryParameter("userName", userName);
        return Client.Execute(request);
    }

    public RestResponse UpdateUserStatus(long userId, string status)
    {
        var request = PrepareRequest($"{BaseUrl}/status/{userId}", Method.Put);
        request.AddQueryParameter("status", status);
        return Client.Execute(request);
    }

    public RestResponse UpdateUserProfile(ProfileUpdateRequestDto requestBody)
    {
        // Swagger: PUT /user/profile
        var request = PrepareRequest($"{base._baseUrl}/user/profile", Method.Put, requestBody);
        return Client.Execute(request);
    }

    public RestResponse DeleteUser()
    {
        var request = PrepareRequest($"{BaseUrl}/delete", Method.Delete);
        return Client.Execute(request);
    }
}
