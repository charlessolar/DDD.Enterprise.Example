
using ServiceStack;

namespace Demo.Presentation.ServiceStack.Authentication.Users.Models
{
    [Api("Users")]
    [Route("/user/avatar", "PUT POST")]
    public class AuChangeAvatar  : Infrastructure.Commands.ServiceCommand
    {
        public string ImageType { get; set; }

        public string ImageData { get; set; }
    }
}