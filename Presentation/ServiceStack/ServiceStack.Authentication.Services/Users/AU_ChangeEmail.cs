using Demo.Presentation.ServiceStack.Infrastructure.Responses;
using ServiceStack;
using System;

namespace Demo.Presentation.ServiceStack.Authentication.Users.Models
{
    [Api("Users")]
    [Route("/user/email", "PUT POST")]
    public class AU_ChangeEmail : IReturnVoid
    {
        public String Email { get; set; }
    }
}