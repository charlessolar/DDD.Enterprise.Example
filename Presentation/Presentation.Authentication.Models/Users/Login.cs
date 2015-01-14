using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Presentation.Authentication.Models.Users
{
    [Api("Authentication")]
    [Route("/user/login", "POST")]
    public class Login : IReturn<Command>
    {
        public String UserId { get; set; }

        public String Name { get; set; }

        public String Email { get; set; }

        public String NickName { get; set; }

        public String ImageUrl { get; set; }
    }
}