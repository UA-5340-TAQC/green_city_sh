using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_city_sh.Tests.Api.DTO
{
    public class FriendResponse
    {
        public long id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public double rating { get; set; }
        public int mutualFriends { get; set; }
        public string? profilePicturePath { get; set; }
        public string friendStatus { get; set; }
        public long requesterId { get; set; }
    }
}
