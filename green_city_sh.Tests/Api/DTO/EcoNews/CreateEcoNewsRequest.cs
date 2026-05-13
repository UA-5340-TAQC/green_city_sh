namespace green_city_sh.Tests.Api.DTO.EcoNews;

public class CreateEcoNewsRequest
{
    public string Title { get; set; }
    public string Text { get; set; }
    public List<string> Tags { get; set; } = new List<string>();
    public string Source { get; set; }
    public string ShortInfo { get; set; }
}