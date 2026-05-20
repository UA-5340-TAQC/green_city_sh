using System;

namespace green_city_sh.Tests.Api.DTO
{
    public class AddCommentRequest
    {
        public string text { get; set; }
        public int parentCommentId { get; set; } = 0;
    }
}
