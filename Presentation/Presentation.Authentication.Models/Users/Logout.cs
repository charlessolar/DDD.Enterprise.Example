using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Presentation.Authentication.Models.Users
{
    [Api("Authentication")]
    [Route("/user/logout", "POST")]
    public class Logout : IReturn<Command>
    {
        // Manual logout, timeout, or other
        public String Event { get; set; }
    }
}