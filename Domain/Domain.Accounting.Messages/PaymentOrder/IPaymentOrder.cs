using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.PaymentOrder
{
    public enum STATE
    {
        NEW,
        CONFIRMED,
        DISPURSED,
        DISCARDED,
        INVALID
    }

    public interface IPaymentOrder : Aggregates.Contracts.IEventSource<Guid>
    {
        Aggregates.SingleValueObject<STATE> State { get; }
    }
}