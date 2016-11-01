using ServiceStack;

namespace Demo.Presentation.ServiceStack.Authentication.Users.Models
{
    [Api("Users")]
    [Route("/user/timezone", "PUT POST")]
    public class AuChangeTimezone  : Infrastructure.Commands.ServiceCommand
    {
        public string Timezone { get; set; }
    }
}