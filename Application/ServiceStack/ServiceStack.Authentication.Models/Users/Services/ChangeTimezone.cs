using Demo.Library.Responses;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Authentication.Users.Services
{
    [Api("Users")]
    [Route("/user/timezone", "PUT POST")]
    public class ChangeTimezone : IReturn<Base<Command>>
    {
        public String Timezone { get; set; }
    }
}