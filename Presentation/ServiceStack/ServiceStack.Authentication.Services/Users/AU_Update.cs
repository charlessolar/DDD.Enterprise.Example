using ServiceStack;

namespace Demo.Presentation.ServiceStack.Authentication.Users.Models
{
    [Api("Users")]
    [Route("/user/{UserAuthId}", "PUT POST")]
    public class AuUpdate
    {
        public string UserAuthId { get; set; }

        public string PrimaryEmail { get; set; }
        public string Nickname { get; set; }
        public string DisplayName { get; set; }
        public string Timezone { get; set; }
    }
}
