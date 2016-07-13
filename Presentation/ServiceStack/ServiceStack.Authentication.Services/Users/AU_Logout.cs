using Demo.Presentation.ServiceStack.Infrastructure.Responses;
using ServiceStack;
using System;

namespace Demo.Presentation.ServiceStack.Authentication.Users.Models
{
    [Api("Authentication")]
    [Route("/user/logout", "POST")]
    public class AU_Logout : IReturnVoid
    {
        // Manual logout, timeout, or other
        public String Event { get; set; }
    }
}