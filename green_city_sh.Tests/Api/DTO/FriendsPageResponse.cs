using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_city_sh.Tests.Api.DTO
{
    public class FriendsPageResponse
    {
        public List<FriendResponse> page { get; set; }
        public int totalElements { get; set; }
        public int currentPage { get; set; }
        public int totalPages { get; set; }
    }
}
