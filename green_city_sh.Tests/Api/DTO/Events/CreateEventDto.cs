namespace green_city_sh.Tests.Api.DTO.Events;

public class CreateEventDto
{
    public string title { get; set; }
    public string description { get; set; }
    public bool open { get; set; }
    public List<DateLocationDto> datesLocations { get; set; }
    public List<string> tags { get; set; }
}

public class DateLocationDto
{
    public DateTime startDate { get; set; }
    public DateTime finishDate { get; set; }
    public CoordinatesDto coordinates { get; set; }
    public string onlineLink { get; set; }
}

public class CoordinatesDto
{
    public double latitude { get; set; }
    public double longitude { get; set; }
}