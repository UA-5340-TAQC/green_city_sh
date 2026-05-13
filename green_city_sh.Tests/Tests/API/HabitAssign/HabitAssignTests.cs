// Tests/API/HabitAssign/HabitAssignTests.cs

using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using green_city_sh.Tests.Api.Clients.GreencityUser;
using green_city_sh.Tests.Api.DTO;
using green_city_sh.Tests.Api.DTO.Habit_assign_controller;
using green_city_sh.Tests.Infrastructure;
using System.Net;
using System.Text.Json;
using green_city_sh.Tests.Api.Clients.Greencity;

namespace green_city_sh.Tests.Tests.API.HabitAssign
{
    /// <summary>
    /// API tests for Habit Assign controller
    /// Verifies functionality of getting assigned habits for current user
    /// </summary>
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

        /// <summary>Sets up test environment, authenticates user and retrieves assigned habits</summary>
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
            var authData = JsonSerializer.Deserialize<AuthResponce>(authResponse.Content ?? string.Empty);
            if (authData?.accessToken == null)
                throw new InvalidOperationException("Failed to authenticate: access token is null");
            var accessToken = authData.accessToken;

            var client = new HabitAssignClient(Configuration.ApiGreenCityBaseUrl, accessToken);
            (_responseStatusCode, _habits) = client.GetAllAssignedHabitsWithData("En");
        }

        /// <summary>Verifies that authenticated user receives HTTP 200 OK response</summary>
        [Test]
        [AllureStory("Get assigned habits for current user")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureDescription("Verify that authenticated user can successfully get assigned habits with status code 200")]
        [AllureTag("API", "HabitAssign", "Positive")]
        public void VerifyGetAllAssignedHabits_ReturnsSuccessStatusCode()
        {
            Assert.That(_responseStatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        /// <summary>Verifies that all assigned habits have status INPROGRESS or ACQUIRED</summary>
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

        /// <summary>Verifies that CreateDateTime and LastEnrollmentDate are not empty</summary>
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

        /// <summary>Verifies that CreateDateTime is not greater than LastEnrollmentDate</summary>
        [Test]
        [AllureStory("Validate date fields")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureDescription("Verify that CreateDateTime is not greater than LastEnrollmentDate")]
        [AllureTag("API", "HabitAssign", "Validation")]
        public void VerifyGetAllAssignedHabits_CreateDateNotGreaterThanLastEnrollmentDate()
        {
            var invalidDates = _habits.Where(h =>
            {
                if (string.IsNullOrEmpty(h.CreateDateTime) || string.IsNullOrEmpty(h.LastEnrollmentDate))
                    return true;

                if (!DateTime.TryParse(h.CreateDateTime, out var createDate) ||
                    !DateTime.TryParse(h.LastEnrollmentDate, out var lastEnrollDate))
                    return true;

                return createDate > lastEnrollDate;
            }).ToList();

            Assert.That(invalidDates, Is.Empty,
                $"CreateDateTime should not be greater than LastEnrollmentDate. Invalid habits: {string.Join(", ", invalidDates.Select(h => h.Id))}");
        }
    }
}