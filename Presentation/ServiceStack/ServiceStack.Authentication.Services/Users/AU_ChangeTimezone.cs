
using Demo.Presentation.ServiceStack.Infrastructure.Responses;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.ServiceStack.Authentication.Users.Models
{
    [Api("Users")]
    [Route("/user/timezone", "PUT POST")]
    public class AU_ChangeTimezone : IReturnVoid
    {
        public String Timezone { get; set; }
    }
}