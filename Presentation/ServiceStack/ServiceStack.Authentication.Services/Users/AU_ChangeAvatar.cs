
using ServiceStack;
using System;
using Demo.Presentation.ServiceStack.Infrastructure.Responses;

namespace Demo.Presentation.ServiceStack.Authentication.Users.Models
{
    [Api("Users")]
    [Route("/user/avatar", "PUT POST")]
    public class AU_ChangeAvatar : IReturnVoid
    {
        public String ImageType { get; set; }

        public String ImageData { get; set; }
    }
}