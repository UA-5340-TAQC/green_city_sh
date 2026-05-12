namespace green_city_sh.Tests.Api.DTO.EcoNews;

public class EcoNewsPageResponse
{
    public int TotalElements { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int Number { get; set; }
    public bool HasPrevious { get; set; }
    public bool HasNext { get; set; }
    public bool First { get; set; }
    public bool Last { get; set; }
}