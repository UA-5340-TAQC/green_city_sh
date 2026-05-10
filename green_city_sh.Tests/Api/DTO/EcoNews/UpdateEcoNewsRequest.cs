namespace green_city_sh.Tests.Api.DTO.EcoNews;

public class UpdateEcoNewsRequest
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string ShortInfo { get; set; }
    public List<string> Tags { get; set; } = new List<string>();
    public string Source { get; set; }
}