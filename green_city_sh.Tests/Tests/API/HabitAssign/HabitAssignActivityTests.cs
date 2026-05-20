// Tests/API/HabitAssign/HabitAssignActivityTests.cs

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
    /// API tests for Habit Assign Activity controller
    /// Verifies functionality of getting user activities between dates
    /// </summary>
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

        /// <summary>Sets up test environment, authenticates user and retrieves activities for last 30 days</summary>
        [SetUp]
        [AllureBefore("Setup: Authenticate and get activities between dates")]
        public void SetUp()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            _fromDate = today.AddDays(-30).ToString("yyyy-MM-dd");
            _toDate = today.ToString("yyyy-MM-dd");

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
            (_responseStatusCode, _activities) = client.GetUserActivitiesWithData(_fromDate, _toDate);
        }

        /// <summary>Verifies that authenticated user receives HTTP 200 OK response for activities endpoint</summary>
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

        /// <summary>Verifies that activities response contains non-empty list</summary>
        [Test]
        [AllureStory("Get user activities between dates")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureDescription("Verify that activities response contains non-empty list")]
        [AllureTag("API", "HabitAssign", "Activity", "Positive")]
        public void VerifyGetUserActivities_ReturnsNonEmptyList()
        {
            Assert.Multiple(() =>
            {
                Assert.That(_activities, Is.Not.Null, "Activities list should not be null");
                Assert.That(_activities, Is.Not.Empty, $"Should have activities between {_fromDate} and {_toDate}");
            });
        }

        /// <summary>Verifies that all activities have valid dates within requested range</summary>
        [Test]
        [AllureStory("Validate activities date range")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureDescription("Verify that all activities have valid dates within requested range")]
        [AllureTag("API", "HabitAssign", "Activity", "Validation")]
        public void VerifyGetUserActivities_ReturnsValidDates()
        {
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

        /// <summary>Verifies that activities are ordered by date ascending</summary>
        [Test]
        [AllureStory("Get user activities between dates")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureDescription("Verify that activities are ordered by date")]
        [AllureTag("API", "HabitAssign", "Activity", "Validation")]
        public void VerifyGetUserActivities_AreOrderedByDate()
        {
            var dates = _activities.Select(a => DateOnly.Parse(a.EnrollDate)).ToList();
            var sortedDates = dates.OrderBy(d => d).ToList();

            Assert.That(dates, Is.EqualTo(sortedDates),
                "Activities should be ordered by enroll date ascending");
        }
    }
}
