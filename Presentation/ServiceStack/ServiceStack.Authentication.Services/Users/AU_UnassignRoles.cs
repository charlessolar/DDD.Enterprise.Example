using ServiceStack;
using System.Collections.Generic;

namespace Demo.Presentation.ServiceStack.Authentication.Users.Models
{
    [Api("Users")]
    [Route("/user/{UserAuthId}/roles", "DELETE")]
    public class AuUnassignRoles
    {
        public string UserAuthId { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public IEnumerable<string> Permissions { get; set; }
    }
}
