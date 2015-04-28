using Demo.Library.Command;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.PaymentOrder.Commands
{
    public class Start : DemoCommand
    {
        public Guid PaymentOrderId { get; set; }

        public String Identity { get; set; }
    }
}