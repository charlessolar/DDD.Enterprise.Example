using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Currency.Events
{
    public interface Created : IEvent
    {
        Guid CurrencyId { get; set; }

        String Code { get; set; }

        String Name { get; set; }

        String Symbol { get; set; }

        Boolean SymbolBefore { get; set; }

        String Format { get; set; }

        String Fraction { get; set; }

        Decimal RoundingFactor { get; set; }

        Int32 ComputationalAccuracy { get; set; }
    }
}