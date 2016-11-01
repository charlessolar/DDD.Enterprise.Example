using ServiceStack;

namespace Demo.Presentation.ServiceStack.Authentication.Users.Models
{
    [Api("Authentication")]
    [Route("/user/login", "POST")]
    public class AuLogin  : Infrastructure.Commands.ServiceCommand
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}