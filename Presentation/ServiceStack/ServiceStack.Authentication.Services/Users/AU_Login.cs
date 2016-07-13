
using Demo.Presentation.ServiceStack.Infrastructure.Responses;
using ServiceStack;
using System;

namespace Demo.Presentation.ServiceStack.Authentication.Users.Models
{
    [Api("Authentication")]
    [Route("/user/login", "POST")]
    public class AU_Login : IReturnVoid
    {
        public String Username { get; set; }
        public String Password { get; set; }
    }
}