namespace green_city_sh.Tests.Api.DTO;

public class SearchEventsResponseDto
{
    public List<SearchEventItemDto> page { get; set; }
    public int totalElements { get; set; }
    public int currentPage { get; set; }
    public int totalPages { get; set; }
}

public class SearchEventItemDto
{
    public long id { get; set; }
    public string title { get; set; }
    public List<string> tags { get; set; }
}
