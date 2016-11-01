using ServiceStack;

namespace Demo.Presentation.ServiceStack.Authentication.Users.Models
{
    [Api("Users")]
    [Route("/user/name", "POST PUT")]
    public class AuChangeName  : Infrastructure.Commands.ServiceCommand
    {
        public string Name { get; set; }
    }
}