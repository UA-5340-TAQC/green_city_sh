namespace green_city_sh.Tests.Api.DTO;

public class SearchPlacesResponseDto
{
    public List<SearchPlaceItemDto> page { get; set; }
    public int totalElements { get; set; }
    public int currentPage { get; set; }
    public int totalPages { get; set; }
}

public class SearchPlaceItemDto
{
    public long id { get; set; }
    public string name { get; set; }
    public string category { get; set; }
}