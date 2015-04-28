using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.Library.Command;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.PaymentOrder.Commands
{
    public class Confirm : DemoCommand
    {
        public Guid PaymentOrderId { get; set; }
    }
}