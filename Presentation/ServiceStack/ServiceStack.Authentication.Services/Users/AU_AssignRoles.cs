using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.ServiceStack.Authentication.Users.Models
{
    [Api("Users")]
    [Route("/user/{UserAuthId}/roles", "PUT POST")]
    public class AU_AssignRoles : IReturnVoid
    {
        public String UserAuthId { get; set; }
        public IEnumerable<String> Roles { get; set; }
        public IEnumerable<String> Permissions { get; set; }
    }
}
