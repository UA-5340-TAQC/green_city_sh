namespace green_city_sh.Tests.Api.DTO.Events;

public class UpdateEventDto
{
    public int id { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public List<DateLocationUpdateDto> datesLocations { get; set; }
    public string titleImage { get; set; }
    public List<string> additionalImages { get; set; }
    public List<string> tags { get; set; }
    public bool open { get; set; }
}

public class DateLocationUpdateDto
{
    public DateTime startDate { get; set; }
    public DateTime finishDate { get; set; }
    public string onlineLink { get; set; }
    public CoordinatesDto coordinates { get; set; }
}
