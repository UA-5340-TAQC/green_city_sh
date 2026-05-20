namespace green_city_sh.Tests.Api.DTO.EcoNews;

public class EcoNewsModel
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string ShortInfo { get; set; }
    public Author Author { get; set; }
    public DateTime CreationDate { get; set; }
    public string ImagePath { get; set; }
    public List<string> TagsUk { get; set; } = new List<string>();
    public List<string> TagsEn { get; set; } = new List<string>();
    public int Likes { get; set; }
    public int CountComments { get; set; }
}
