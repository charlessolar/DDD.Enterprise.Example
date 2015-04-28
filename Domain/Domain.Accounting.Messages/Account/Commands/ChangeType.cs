using Demo.Library.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Account.Commands
{
    public class ChangeType : DemoCommand
    {
        public Guid AccountId { get; set; }
        public Guid TypeId { get; set; }
    }
}
