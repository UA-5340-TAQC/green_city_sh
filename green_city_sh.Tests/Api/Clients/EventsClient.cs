using green_city_sh.Tests.Api.DTO.Events;
using green_city_sh.Tests.Infrastructure;
using RestSharp;
using System.Text;
using System.Text.Json;

namespace green_city_sh.Tests.Api.Clients;

internal class EventsClient : BaseApiClient
{
    protected override string BaseUrl => $"{base._baseUrl}/events";

    public EventsClient(string apiGreenCityBaseUrl, string? authToken = null) : base(apiGreenCityBaseUrl, authToken) { }

    public RestResponse GetAllEvents(int userId, int page = 0, int size = 5)
    {
        var request = new RestRequest("", Method.Get);
        request.AddQueryParameter("user-id", userId);
        request.AddQueryParameter("page", page);
        request.AddQueryParameter("size", size);

        if (!string.IsNullOrEmpty(AuthToken))
        {
            request.AddHeader("Authorization", $"Bearer {AuthToken}");
        }

        return Client.Execute(request);
    }

    public RestResponse GetEventById(int eventId)
    {
        var request = new RestRequest($"/{eventId}", Method.Get);

        if (!string.IsNullOrEmpty(AuthToken))
        {
            request.AddHeader("Authorization", $"Bearer {AuthToken}");
        }

        return Client.Execute(request);
    }

    public RestResponse CreateEvent(CreateEventDto eventData)
    {
        var client = new RestClient(Configuration.ApiGreenCityBaseUrl);
        var request = new RestRequest("/events", Method.Post);

        request.AddHeader("Authorization", $"Bearer {AuthToken}");
        request.AddHeader("Accept", "application/json");

        var addEventDtoRequest = new
        {
            title = eventData.title,
            description = eventData.description,
            open = eventData.open,
            datesLocations = eventData.datesLocations.Select(d => new
            {
                startDate = d.startDate.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                finishDate = d.finishDate.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                coordinates = new { latitude = d.coordinates.latitude, longitude = d.coordinates.longitude },
                onlineLink = d.onlineLink
            }),
            tags = eventData.tags
        };

        var json = JsonSerializer.Serialize(addEventDtoRequest);

        request.AddFile("addEventDtoRequest", Encoding.UTF8.GetBytes(json), "addEventDtoRequest.json", "application/json");
        request.AlwaysMultipartFormData = true;

        return client.Execute(request);
    }

    public RestResponse UpdateEvent(int eventId, UpdateEventDto eventData)
    {
        var client = new RestClient(Configuration.ApiGreenCityBaseUrl);
        var request = new RestRequest($"/events/{eventId}", Method.Put);

        request.AddHeader("Authorization", $"Bearer {AuthToken}");
        request.AddHeader("Accept", "*/*");

        var json = JsonSerializer.Serialize(eventData);
        request.AddFile("eventDto", Encoding.UTF8.GetBytes(json), "eventDto.json", "application/json");
        request.AlwaysMultipartFormData = true;

        return client.Execute(request);
    }

    public RestResponse DeleteEvent(int eventId)
    {
        var client = new RestClient(Configuration.ApiGreenCityBaseUrl);
        var request = new RestRequest($"/events/{eventId}", Method.Delete);

        request.AddHeader("Authorization", $"Bearer {AuthToken}");
        request.AddHeader("Accept", "*/*");

        return client.Execute(request);
    }
}