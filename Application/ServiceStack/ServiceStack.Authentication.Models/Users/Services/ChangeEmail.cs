using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Authentication.Users.Services
{
    [Api("Users")]
    [Route("/user/email", "PUT POST")]
    public class ChangeEmail : IReturn<Base<Command>>
    {
        public String Email { get; set; }
    }
}