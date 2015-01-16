using Forte.Library.Responses;
using ServiceStack;
using System;

namespace Forte.Application.ServiceStack.Authentication.Models.Users
{
    [Api("Users")]
    [Route("/user/email", "PUT, POST")]
    public class ChangeEmail : IReturn<Command>
    {
        public String Email { get; set; }
    }
}