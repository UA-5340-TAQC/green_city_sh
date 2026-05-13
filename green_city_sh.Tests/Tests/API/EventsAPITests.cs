using Allure.NUnit;
using Allure.NUnit.Attributes;
using green_city_sh.Tests.Api.Clients;
using green_city_sh.Tests.Api.Clients.GreencityUser;
using green_city_sh.Tests.Api.DTO;
using green_city_sh.Tests.Api.DTO.Events;
using green_city_sh.Tests.Infrastructure;
using RestSharp;
using System.Net;
using System.Text.Json;

namespace green_city_sh.Tests.Tests.API;

[TestFixture]
[Allure.NUnit.AllureNUnit]
[Parallelizable(ParallelScope.Self)]
public class EventsAPITests
{
    private OwnSecurityClient _authClient;
    private EventsClient _eventsClient;
    private string _accessToken;
    private int _userId;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _authClient = new OwnSecurityClient(Configuration.ApiUserBaseUrl);

        var signInModel = new SignInModal
        {
            email = Configuration.TestEmail,
            password = Configuration.TestPassword
        };

        var authResponse = _authClient.SignIn(signInModel);
        Assert.That(authResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Authentication failed");
        Assert.That(authResponse.Content, Is.Not.Null, "Authentication response content should not be null");

        var authData = JsonSerializer.Deserialize<AuthResponce>(authResponse.Content);
        Assert.That(authData, Is.Not.Null, "Failed to deserialize auth response");

        _accessToken = authData.accessToken;
        _userId = authData.userId;

        Assert.That(_accessToken, Is.Not.Null.Or.Empty, "Access token should not be null or empty");
        Assert.That(_userId, Is.GreaterThan(0), "User ID should be positive");

        _eventsClient = new EventsClient(Configuration.ApiGreenCityBaseUrl, _accessToken);
        TestContext.WriteLine($"Access token received: {(string.IsNullOrEmpty(_accessToken) ? "NO" : "YES")}");
        TestContext.WriteLine($"Token length: {_accessToken?.Length ?? 0}");
    }

    [Test]
    [Order(1)]
    [AllureIssue("EVENTS-1")]
    [AllureDescription("Verify that authenticated user can get the first event from the list")]
    public void VerifyGetFirstEvent_Success()
    {
        var eventsResponse = _eventsClient.GetAllEvents(_userId, 0, 1);
        Assert.That(eventsResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK),
            "Failed to get events list");
        Assert.That(eventsResponse.Content, Is.Not.Null, "Events response content should not be null");

        var eventsList = JsonSerializer.Deserialize<EventsListResponse>(eventsResponse.Content);
        Assert.That(eventsList, Is.Not.Null, "Failed to deserialize events list");
        Assert.That(eventsList.page, Is.Not.Empty, "Events list should not be empty");

        var firstEvent = eventsList.page[0];
        Assert.That(firstEvent, Is.Not.Null, "First event should not be null");
        Assert.That(firstEvent.id, Is.GreaterThan(0), "Event ID should be positive");

        var eventResponse = _eventsClient.GetEventById(firstEvent.id);
        Assert.That(eventResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK),
            $"Failed to get event with ID: {firstEvent.id}");
        Assert.That(eventResponse.Content, Is.Not.Null, $"Event response content for ID {firstEvent.id} should not be null");

        var eventDetails = JsonSerializer.Deserialize<EventResponse>(eventResponse.Content);
        Assert.That(eventDetails, Is.Not.Null, $"Failed to deserialize event with ID: {firstEvent.id}");

        Assert.Multiple(() =>
        {
            Assert.That(eventDetails.id, Is.EqualTo(firstEvent.id), "Event ID mismatch");
            Assert.That(eventDetails.title, Is.EqualTo(firstEvent.title), "Event title mismatch");
            Assert.That(eventDetails.type, Is.EqualTo(firstEvent.type), "Event type mismatch");
            Assert.That(eventDetails.open, Is.EqualTo(firstEvent.open), "Event open status mismatch");
            Assert.That(eventDetails.creationDate, Is.Not.Empty, "Creation date should not be empty");
            Assert.That(eventDetails.description, Is.Not.Null, "Description should not be null");
            Assert.That(eventDetails.organizer, Is.Not.Null, "Organizer should not be null");
            Assert.That(eventDetails.organizer.name, Is.Not.Empty, "Organizer name should not be empty");
        });
    }

    [Test]
    [Order(2)]
    [AllureIssue("EVENTS-2")]
    [AllureDescription("Verify that getting event with non-existent ID returns 404")]
    public void VerifyGetNonExistentEvent_ReturnsNotFound()
    {
        int nonExistentEventId = 999999;
        var response = _eventsClient.GetEventById(nonExistentEventId);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound),
            $"Expected 404 for non-existent event ID: {nonExistentEventId}");
    }

    [Test]
    [Order(3)]
    [AllureIssue("EVENTS-3")]
    [AllureDescription("Create event - verify event can be created successfully")]
    public void CreateEvent_Success()
    {
        var (eventId, _) = CreateTestEventAndGetStatus("Test Event for Creation", "This event will be created and verified");
        var getResponse = _eventsClient.GetEventById(eventId);

        Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Event should exist after creation");

        _eventsClient.DeleteEvent(eventId);
    }

    [Test]
    [Order(4)]
    [AllureIssue("EVENTS-4")]
    [AllureDescription("Update event status - toggle between open and close")]
    public void UpdateEventStatus_Success()
    {
        var (eventId, currentStatus) = CreateTestEventAndGetStatus("Test Event for Update", "This event will be used to test update");
        bool newStatus = !currentStatus;
        var getResponse = _eventsClient.GetEventById(eventId);
        var existingEvent = JsonSerializer.Deserialize<EventResponse>(getResponse.Content);

        var updateData = new UpdateEventDto
        {
            id = eventId,
            title = existingEvent.title,
            description = existingEvent.description,
            open = newStatus,
            datesLocations = existingEvent.dates.Select(d => new DateLocationUpdateDto
            {
                startDate = d.startDate,
                finishDate = d.finishDate,
                onlineLink = d.onlineLink,
                coordinates = new CoordinatesDto
                {
                    latitude = d.coordinates.latitude,
                    longitude = d.coordinates.longitude
                }
            }).ToList(),
            titleImage = existingEvent.titleImage ?? "",
            additionalImages = existingEvent.additionalImages ?? new List<string>(),
            tags = existingEvent.tags.Select(t => t.nameEn).ToList()
        };

        var updateResponse = _eventsClient.UpdateEvent(eventId, updateData);
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Failed to update event");

        var updatedEvent = JsonSerializer.Deserialize<EventResponse>(updateResponse.Content);
        Assert.That(updatedEvent.open, Is.EqualTo(newStatus), $"Status should be toggled from {currentStatus} to {newStatus}");

        _eventsClient.DeleteEvent(eventId);
    }

    [Test]
    [Order(5)]
    [AllureIssue("EVENTS-5")]
    [AllureDescription("Delete event - verify event can be deleted successfully")]
    public void DeleteEvent_Success()
    {
        var (eventId, _) = CreateTestEventAndGetStatus("Test Event for Deletion", "This event will be deleted");

        var deleteResponse = _eventsClient.DeleteEvent(eventId);
        Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Failed to delete event");

        var getResponse = _eventsClient.GetEventById(eventId);
        Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound), "Event should not exist after deletion");
    }

    private CreateEventDto CreateTestEventDto(string title, string description, bool open = true, float latitude = 48.491673f, float longitude = 35.104595f, string onlineLink = "https://youtube.com")
    {
        return new CreateEventDto
        {
            title = title,
            description = description,
            open = open,
            datesLocations = new List<DateLocationDto>
        {
            new DateLocationDto
            {
                startDate = DateTime.UtcNow.AddDays(30),
                finishDate = DateTime.UtcNow.AddDays(30).AddHours(2),
                coordinates = new CoordinatesDto
                {
                    latitude = latitude,
                    longitude = longitude
                },
                onlineLink = onlineLink
            }
        },
            tags = new List<string> { "Social" }
        };
    }

    private (int eventId, bool isOpen) CreateTestEventAndGetStatus(string title, string description)
    {
        var createData = CreateTestEventDto(title, description);
        var createResponse = _eventsClient.CreateEvent(createData);
        Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created), $"Failed to create event: {title}");
        Assert.That(createResponse.Content, Is.Not.Null, "Create response content should not be null");

        var createdEvent = JsonSerializer.Deserialize<EventResponse>(createResponse.Content);
        Assert.That(createdEvent, Is.Not.Null, "Failed to deserialize created event");
        Assert.That(createdEvent.id, Is.GreaterThan(0), "Created event ID should be positive");

        return (createdEvent.id, createdEvent.open);
    }
}
