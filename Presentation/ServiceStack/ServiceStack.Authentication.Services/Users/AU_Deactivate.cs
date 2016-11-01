using ServiceStack;

namespace Demo.Presentation.ServiceStack.Authentication.Users
{
    [Api("Users")]
    [Route("/user/{UserAuthId}/deactivate", "PUT POST")]
    public class AuDeactivate
    {
        public string UserAuthId { get; set; }
    }
}
