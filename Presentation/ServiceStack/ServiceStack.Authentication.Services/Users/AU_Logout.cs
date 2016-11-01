using ServiceStack;

namespace Demo.Presentation.ServiceStack.Authentication.Users.Models
{
    [Api("Authentication")]
    [Route("/user/logout", "POST")]
    public class AuLogout  : Infrastructure.Commands.ServiceCommand
    {
        // Manual logout, timeout, or other
        public string Event { get; set; }
    }
}