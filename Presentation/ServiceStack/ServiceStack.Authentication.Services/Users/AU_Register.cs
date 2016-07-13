using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.ServiceStack.Authentication.Users.Models
{
    [Api("Users")]
    [Route("/user/register", "POST")]
    public class AU_Register : IReturnVoid
    {
        public String UserAuthId { get; set; }
        public String Password { get; set; }
    }
}
