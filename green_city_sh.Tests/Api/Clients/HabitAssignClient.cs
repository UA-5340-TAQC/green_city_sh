using Allure.Net.Commons.Attributes;
using green_city_sh.Tests.Api.DTO.Habit_assign_controller;
using RestSharp;
using System.Text.Json;

namespace green_city_sh.Tests.Api.Clients.GreencityUser
{
    public class HabitAssignClient : BaseApiClient
    {
        protected override string BaseUrl => $"{base._baseUrl}/habit/assign";

        public HabitAssignClient(string apiBaseUrl, string? authToken = null) : base(apiBaseUrl, authToken) { }

        [AllureStep("Get all assigned habits for current user")]
        public RestResponse GetAllAssignedHabitsForCurrentUser(string lang = "En")
        {
            var request = PrepareRequest("/allForCurrentUser", Method.Get);
            request.AddQueryParameter("lang", lang);
            return Client.Execute(request);
        }

        [AllureStep("Get user activities between {fromDate} and {toDate}")]
        public RestResponse GetUserActivitiesBetweenDates(string fromDate, string toDate, string lang = "en")
        {
            var request = PrepareRequest($"/activity/{fromDate}/to/{toDate}", Method.Get);
            request.AddQueryParameter("lang", lang);
            return Client.Execute(request);
        }

        [AllureStep("Deserialize habits response")]
        public List<GetAllAssignedHabitsResponse>? DeserializeHabitsResponse(string content)
        {
            if (string.IsNullOrEmpty(content))
                return new List<GetAllAssignedHabitsResponse>();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<List<GetAllAssignedHabitsResponse>>(content, options)
                   ?? new List<GetAllAssignedHabitsResponse>();
        }

        [AllureStep("Deserialize activities response")]
        public List<HabitActivityResponse>? DeserializeActivityResponse(string content)
        {
            if (string.IsNullOrEmpty(content))
                return new List<HabitActivityResponse>();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<List<HabitActivityResponse>>(content, options)
                   ?? new List<HabitActivityResponse>();
        }
    }
}