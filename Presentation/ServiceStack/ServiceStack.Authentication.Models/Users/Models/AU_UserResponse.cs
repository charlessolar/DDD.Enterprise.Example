using ServiceStack.Model;
using System;
using System.Collections.Generic;

namespace Demo.Presentation.ServiceStack.Authentication.Users.Models
{
    public class AuUserResponse : IHasStringId
    {
        public string Id { get; set; }
        public Guid? EmployeeId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Nickname { get; set; }

        public string Timezone { get; set; }

        public string ImageType { get; set; }

        public string ImageData { get; set; }


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