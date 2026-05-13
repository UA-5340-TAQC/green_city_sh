using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_city_sh.Tests.Api.DTO
{
    public class AuthResponce
    {
        public int userId { get; set; }
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
        public string name { get; set; }
        public bool ownRegistrations { get; set; }
    }
}
