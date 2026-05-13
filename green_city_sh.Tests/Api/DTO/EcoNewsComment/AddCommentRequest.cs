namespace green_city_sh.Tests.Api.DTO.EcoNewsComment;

public class AddCommentRequest
{
    public string Text { get; set; }

    public long? ParentCommentId { get; set; }
}