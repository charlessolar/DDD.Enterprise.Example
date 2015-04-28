using Demo.Library.Command;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Account.Commands
{
    public class Create : DemoCommand
    {
        public Guid AccountId { get; set; }

        public String Code { get; set; }

        public String Name { get; set; }

        public Boolean AcceptPayments { get; set; }

        public Boolean AllowReconcile { get; set; }

        public Guid CurrencyId { get; set; }

        public OPERATION Operation { get; set; }

        public Guid StoreId { get; set; }
    }
}