using System.Collections.Generic;

namespace green_city_sh.Tests.Api.DTO.Habits;

public class CommentAuthorDto
{
    public int id { get; set; }
    public string name { get; set; }
    public string profilePicturePath { get; set; }
}

public class HabitCommentResponse
{
    public int id { get; set; }
    public string createdDate { get; set; }
    public string modifiedDate { get; set; }
    public CommentAuthorDto author { get; set; }
    public int parentCommentId { get; set; }
    public string text { get; set; }
    public int replies { get; set; }
    public int likes { get; set; }
    public int dislikes { get; set; }
    public bool currentUserLiked { get; set; }
    public bool currentUserDisliked { get; set; }
    public string status { get; set; }
    public List<string> additionalImages { get; set; }
}

public class HabitCommentPageResponse
{
    public List<HabitCommentResponse> page { get; set; }
    public int totalElements { get; set; }
    public int currentPage { get; set; }
    public int totalPages { get; set; }
}