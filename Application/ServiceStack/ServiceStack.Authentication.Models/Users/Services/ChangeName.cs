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
    [Route("/user/name", "POST PUT")]
    public class ChangeName : IReturn<Base<Command>>
    {
        public String Name { get; set; }
    }
}