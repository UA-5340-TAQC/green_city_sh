// Api/Clients/GreencityUser/HabitAssignClient.cs

using Allure.Net.Commons.Attributes;
using green_city_sh.Tests.Api.DTO.Habit_assign_controller;
using RestSharp;
using System.Net;
using System.Text.Json;

namespace green_city_sh.Tests.Api.Clients
{
    /// <summary>
    /// Client for HabitAssign API endpoints
    /// </summary>
    public class HabitAssignClient : BaseApiClient
    {
        protected override string BaseUrl => $"{base._baseUrl}/habit/assign";

        public HabitAssignClient(string apiBaseUrl, string? authToken = null) : base(apiBaseUrl, authToken) { }

        /// <summary>
        /// Gets all assigned habits for the current authenticated user
        /// </summary>
        /// <param name="lang">Language code (default: "en")</param>
        /// <returns>Tuple containing HTTP status code and list of assigned habits</returns>
        [AllureStep("Get all assigned habits for current user")]
        public (HttpStatusCode StatusCode, List<GetAllAssignedHabitsResponse> Habits) GetAllAssignedHabitsWithData(string lang = "en")
        {
            var client = new RestClient(BaseUrl);
            var request = new RestRequest("/allForCurrentUser", Method.Get);
            request.AddQueryParameter("lang", lang);
            request.AddHeader("Authorization", $"Bearer {AuthToken}");
            request.AddHeader("accept", "*/*");

            var response = client.Execute(request);
            var statusCode = response.StatusCode;

            if (statusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(response.Content))
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var habits = JsonSerializer.Deserialize<List<GetAllAssignedHabitsResponse>>(response.Content, options);
                return (statusCode, habits ?? new List<GetAllAssignedHabitsResponse>());
            }
            else
            {
                return (statusCode, new List<GetAllAssignedHabitsResponse>());
            }
        }

        /// <summary>
        /// Gets user activities between specified dates
        /// </summary>
        /// <param name="fromDate">Start date in format YYYY-MM-DD</param>
        /// <param name="toDate">End date in format YYYY-MM-DD</param>
        /// <param name="lang">Language code (default: "en")</param>
        /// <returns>Tuple containing HTTP status code and list of activities</returns>
        [AllureStep("Get user activities between {fromDate} and {toDate}")]
        public (HttpStatusCode StatusCode, List<HabitActivityResponse> Activities) GetUserActivitiesWithData(string fromDate, string toDate, string lang = "en")
        {
            var client = new RestClient(BaseUrl);
            var request = new RestRequest($"/activity/{fromDate}/to/{toDate}", Method.Get);
            request.AddQueryParameter("lang", lang);
            request.AddHeader("Authorization", $"Bearer {AuthToken}");
            request.AddHeader("accept", "*/*");

            var response = client.Execute(request);
            var statusCode = response.StatusCode;

            if (statusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(response.Content))
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var activities = JsonSerializer.Deserialize<List<HabitActivityResponse>>(response.Content, options);
                return (statusCode, activities ?? new List<HabitActivityResponse>());
            }
            else
            {
                return (statusCode, new List<HabitActivityResponse>());
            }
        }
    }
}