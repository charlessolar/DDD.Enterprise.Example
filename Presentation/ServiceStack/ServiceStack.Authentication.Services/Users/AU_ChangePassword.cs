using ServiceStack;

namespace Demo.Presentation.ServiceStack.Authentication.Users.Models
{
    [Api("Users")]
    [Route("/user/{UserAuthId}/avatar", "PUT POST")]
    public class AuChangePassword  : Infrastructure.Commands.ServiceCommand
    {
        public string UserAuthId { get; set; }

        public string Password { get; set; }
    }
}
