using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Account.Events
{
    public interface Created : IEvent
    {
        Guid AccountId { get; set; }

        String Code { get; set; }

        String Name { get; set; }

        Boolean AcceptPayments { get; set; }

        Boolean AllowReconcile { get; set; }

        String Operation { get; set; }


        Guid CurrencyId { get; set; }
    }
}