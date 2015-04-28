using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Authentication.Users.Services
{
    [Api("Users")]
    [Route("/user/avatar", "PUT POST")]
    public class ChangeAvatar : IReturn<Base<Command>>
    {
        public String ImageType { get; set; }

        public String ImageData { get; set; }
    }
}