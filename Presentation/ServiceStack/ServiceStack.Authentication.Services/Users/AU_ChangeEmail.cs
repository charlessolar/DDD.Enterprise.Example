using ServiceStack;

namespace Demo.Presentation.ServiceStack.Authentication.Users.Models
{
    [Api("Users")]
    [Route("/user/email", "PUT POST")]
    public class AuChangeEmail  : Infrastructure.Commands.ServiceCommand
    {
        public string Email { get; set; }
    }
}