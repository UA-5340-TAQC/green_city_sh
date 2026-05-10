namespace green_city_sh.Tests.Api.DTO.Events;

public class EventItem
{
    public int id { get; set; }
    public string title { get; set; }
    public string type { get; set; }
    public bool open { get; set; }
    public int likes { get; set; }
    public int dislikes { get; set; }
}

public class EventsListResponse
{
    public List<EventItem> page { get; set; }
    public int totalElements { get; set; }
    public int currentPage { get; set; }
    public int totalPages { get; set; }
    public bool hasPrevious { get; set; }
    public bool hasNext { get; set; }
}