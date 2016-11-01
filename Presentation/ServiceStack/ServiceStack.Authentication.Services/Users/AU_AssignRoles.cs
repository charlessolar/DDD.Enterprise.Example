using ServiceStack;
using System.Collections.Generic;

namespace Demo.Presentation.ServiceStack.Authentication.Users.Models
{
    [Api("Users")]
    [Route("/user/{UserAuthId}/roles", "PUT POST")]
    public class AuAssignRoles  : Infrastructure.Commands.ServiceCommand
    {
        public string UserAuthId { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public IEnumerable<string> Permissions { get; set; }
    }
}
