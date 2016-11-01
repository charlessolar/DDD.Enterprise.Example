using ServiceStack;

namespace Demo.Presentation.ServiceStack.Authentication.Users.Models
{
    [Api("Users")]
    [Route("/user/register", "POST")]
    public class AuRegister  : Infrastructure.Commands.ServiceCommand
    {
        public string UserAuthId { get; set; }
        public string Password { get; set; }
    }
}
