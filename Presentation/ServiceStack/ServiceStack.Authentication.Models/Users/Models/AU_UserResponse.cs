

using ServiceStack.Auth;
using ServiceStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.ServiceStack.Authentication.Users.Models
{
    public class AU_UserResponse : IHasStringId
    {
        public String Id { get; set; }
        public Guid? EmployeeId { get; set; }

        public String Name { get; set; }

        public String Email { get; set; }

        public String Nickname { get; set; }

        public String Timezone { get; set; }

        public String ImageType { get; set; }

        public String ImageData { get; set; }


        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        
        public string Password { get; set; }
        
        public int InvalidLoginAttempts { get; set; }
        public DateTime? LastLoginAttempt { get; set; }
        public DateTime? LockedDate { get; set; }
        public IEnumerable<string> Permissions { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}