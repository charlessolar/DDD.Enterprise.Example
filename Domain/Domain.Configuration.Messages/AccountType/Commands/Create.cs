using Demo.Library.Command;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.AccountType.Commands
{
    public class Create : DemoCommand
    {
        public Guid AccountTypeId { get; set; }

        public String Name { get; set; }

        public DEFERRAL_METHOD DeferralMethod { get; set; }

        public Guid? ParentId { get; set; }
    }
}