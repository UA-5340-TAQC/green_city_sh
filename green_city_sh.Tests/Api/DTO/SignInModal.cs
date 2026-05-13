using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_city_sh.Tests.Api.DTO
{
    public class SignInModal
    {
        public string email { get; set; }
        public string password { get; set; }
        public string projectName { get; set; }

        public SignInModal()
        {
            this.projectName = "GREENCITY";
        }
    }
}
