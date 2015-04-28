using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Authentication.Users.Services
{
    [Api("Authentication")]
    [Route("/user/logout", "POST")]
    public class Logout : IReturn<Base<Command>>
    {
        // Manual logout, timeout, or other
        public String Event { get; set; }
    }
}