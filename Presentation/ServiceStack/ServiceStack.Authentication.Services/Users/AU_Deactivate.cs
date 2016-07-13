using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.ServiceStack.Authentication.Users
{
    [Api("Users")]
    [Route("/user/{UserAuthId}/deactivate", "PUT POST")]
    public class AU_Deactivate
    {
        public String UserAuthId { get; set; }
    }
}
