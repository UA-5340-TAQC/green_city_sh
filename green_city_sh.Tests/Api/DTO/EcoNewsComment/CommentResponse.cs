namespace green_city_sh.Tests.Api.DTO.EcoNewsComment;

public class EcoNewsCommentResponse
{
    public long Id { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }

    public Author Author { get; set; }

    public long? ParentCommentId { get; set; }

    public string Text { get; set; }
    public int Replies { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public bool CurrentUserLiked { get; set; }
    public bool CurrentUserDisliked { get; set; }
    public string Status { get; set; }

    public List<string> AdditionalImages { get; set; }
}
