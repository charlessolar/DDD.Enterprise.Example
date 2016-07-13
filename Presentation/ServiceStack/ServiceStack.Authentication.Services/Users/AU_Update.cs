using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.ServiceStack.Authentication.Users.Models
{
    [Api("Users")]
    [Route("/user/{UserAuthId}", "PUT POST")]
    public class AU_Update
    {
        public String UserAuthId { get; set; }

        public String PrimaryEmail { get; set; }
        public String Nickname { get; set; }
        public String DisplayName { get; set; }
        public String Timezone { get; set; }
    }
}
