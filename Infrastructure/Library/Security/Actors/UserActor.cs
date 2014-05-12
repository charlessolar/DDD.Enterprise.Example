using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security.Actors
{
    public class UserActor : Actor
    {
        public UserActor()
            : base("USER")
        {
        }

        public IEnumerable<String> Permissions { get; set; }
    }
}