using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using green_city_sh.Tests.Api.Clients.GreencityUser;
using green_city_sh.Tests.Api.DTO;
using green_city_sh.Tests.Api.DTO.Habit_assign_controller;
using green_city_sh.Tests.Infrastructure;
using RestSharp;
using System.Net;
using System.Text.Json;

namespace green_city_sh.Tests.Tests.API.HabitAssign
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("HabitAssign Activity API Tests")]
    [AllureFeature("Habit Activities")]
    [AllureEpic("Habit Management")]
    [Parallelizable(ParallelScope.Self)]
    public class HabitAssignActivityAPITests
    {
        private List<HabitActivityResponse>? _activities;
        private HttpStatusCode _responseStatusCode;
        private string _fromDate;
        private string _toDate;

        [SetUp]
        [AllureBefore("Setup: Authenticate and get activities between dates")]
        public void SetUp()
        {
            _fromDate = "2026-05-13";
            _toDate = "2026-05-30";

            var authClient = new OwnSecurityClient(Configuration.ApiUserBaseUrl);
            var signInModel = new SignInModal
            {
                email = Configuration.TestEmail,
                password = Configuration.TestPassword
            };

            var authResponse = authClient.SignIn(signInModel);
            var authData = JsonSerializer.Deserialize<AuthResponce>(authResponse.Content);
            var accessToken = authData.accessToken;

            var client = new RestClient(Configuration.ApiGreenCityBaseUrl);
            var request = new RestRequest($"/habit/assign/activity/{_fromDate}/to/{_toDate}", Method.Get);
            request.AddQueryParameter("lang", "en");
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            request.AddHeader("accept", "*/*");

            var response = client.Execute(request);
            _responseStatusCode = response.StatusCode;

            if (_responseStatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(response.Content))
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                _activities = JsonSerializer.Deserialize<List<HabitActivityResponse>>(response.Content, options);
            }
            else
            {
                _activities = new List<HabitActivityResponse>();
            }
        }

        [Test]
        [AllureStory("Get user activities between dates")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureDescription("Verify that authenticated user can successfully get activities between specified dates")]
        [AllureTag("API", "HabitAssign", "Activity", "Positive")]
        public void VerifyGetUserActivities_ReturnsSuccessStatusCode()
        {
            Assert.That(_responseStatusCode, Is.EqualTo(HttpStatusCode.OK),
                $"API should return 200 OK. Actual: {_responseStatusCode}");
        }

        [Test]
        [AllureStory("Get user activities between dates")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureDescription("Verify that activities response contains non-empty list")]
        [AllureTag("API", "HabitAssign", "Activity", "Positive")]
        public void VerifyGetUserActivities_ReturnsNonEmptyList()
        {
            Assert.That(_responseStatusCode, Is.EqualTo(HttpStatusCode.OK),
                "Cannot verify list when API returned error");

            Assert.Multiple(() =>
            {
                Assert.That(_activities, Is.Not.Null, "Activities list should not be null");
                Assert.That(_activities, Is.Not.Empty, $"Should have activities between {_fromDate} and {_toDate}");
            });
        }

        [Test]
        [AllureStory("Validate activities date range")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureDescription("Verify that all activities have valid dates within requested range")]
        [AllureTag("API", "HabitAssign", "Activity", "Validation")]
        public void VerifyGetUserActivities_ReturnsValidDates()
        {
            Assert.That(_responseStatusCode, Is.EqualTo(HttpStatusCode.OK),
                "Cannot validate dates when API returned error");

            var expectedStartDate = DateOnly.Parse(_fromDate);
            var expectedEndDate = DateOnly.Parse(_toDate);

            var invalidDates = _activities.Where(a =>
            {
                if (!DateOnly.TryParse(a.EnrollDate, out var actualDate))
                    return true;
                return actualDate < expectedStartDate || actualDate > expectedEndDate;
            }).ToList();

            Assert.That(invalidDates, Is.Empty,
                $"All enroll dates should be between {_fromDate} and {_toDate}. Invalid dates: {string.Join(", ", invalidDates.Select(i => i.EnrollDate))}");
        }

        [Test]
        [AllureStory("Get user activities between dates")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureDescription("Verify that activities are ordered by date")]
        [AllureTag("API", "HabitAssign", "Activity", "Validation")]
        public void VerifyGetUserActivities_AreOrderedByDate()
        {
            Assert.That(_responseStatusCode, Is.EqualTo(HttpStatusCode.OK),
                "Cannot validate order when API returned error");

            var dates = _activities.Select(a => a.EnrollDate).ToList();
            var sortedDates = dates.OrderBy(d => d).ToList();

            Assert.That(dates, Is.EqualTo(sortedDates),
                "Activities should be ordered by enroll date ascending");
        }
    }
}