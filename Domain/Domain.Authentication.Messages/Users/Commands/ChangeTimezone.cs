using Demo.Library.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Authentication.Users.Commands
{
    public class ChangeTimezone : DemoCommand
    {
        public String Timezone { get; set; }
    }
}