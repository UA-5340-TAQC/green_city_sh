
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
    [AllureSuite("HabitAssign API Tests")]
    [AllureFeature("Habit Assignments")]
    [AllureEpic("Habit Management")]
    [Parallelizable(ParallelScope.Self)]
    public class HabitAssignAPITests
    {
        private List<GetAllAssignedHabitsResponse> _habits;
        private HttpStatusCode _responseStatusCode;

        [SetUp]
        [AllureBefore("Setup: Authenticate and get assigned habits")]
        public void SetUp()
        {
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
            var request = new RestRequest("/habit/assign/allForCurrentUser", Method.Get);
            request.AddQueryParameter("lang", "En");
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            request.AddHeader("accept", "*/*");

            var response = client.Execute(request);
            _responseStatusCode = response.StatusCode;

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _habits = JsonSerializer.Deserialize<List<GetAllAssignedHabitsResponse>>(response.Content, options);
        }

        [Test]
        [AllureStory("Get assigned habits for current user")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureDescription("Verify that authenticated user can successfully get assigned habits with status code 200")]
        [AllureTag("API", "HabitAssign", "Positive")]
        public void VerifyGetAllAssignedHabits_ReturnsSuccessStatusCode()
        {
            Assert.That(_responseStatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        [AllureStory("Validate habit statuses")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureDescription("Verify that all assigned habits have status INPROGRESS or ACQUIRED")]
        [AllureTag("API", "HabitAssign", "Validation")]
        public void VerifyGetAllAssignedHabits_ContainsOnlyInprogressOrAcquiredStatus()
        {
            var validStatuses = new[] { "INPROGRESS", "ACQUIRED" };
            Assert.That(_habits.All(h => validStatuses.Contains(h.Status)),
                Is.True, $"Found statuses: {string.Join(", ", _habits.Select(h => h.Status))}");
        }

        [Test]
        [AllureStory("Validate date fields")]
        [AllureSeverity(SeverityLevel.minor)]
        [AllureDescription("Verify that CreateDateTime and LastEnrollmentDate are not empty")]
        [AllureTag("API", "HabitAssign", "Validation")]
        public void VerifyGetAllAssignedHabits_ReturnsValidDates()
        {
            Assert.That(_habits.All(h => !string.IsNullOrEmpty(h.CreateDateTime)), Is.True, "CreateDateTime should not be empty");
            Assert.That(_habits.All(h => !string.IsNullOrEmpty(h.LastEnrollmentDate)), Is.True, "LastEnrollmentDate should not be empty");
        }
    }
}