using Demo.Library.Responses;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Authentication.Models.Users
{
    [Api("Users")]
    [Route("/user/name", "PUT, POST")]
    public class ChangeName : IReturn<Command>
    {
        public String Name { get; set; }
    }
}