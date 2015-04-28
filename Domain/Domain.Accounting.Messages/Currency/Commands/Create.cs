using Demo.Library.Command;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Currency.Commands
{
    public class Create : DemoCommand
    {
        public Guid CurrencyId { get; set; }

        public String Code { get; set; }

        public String Name { get; set; }

        public String Symbol { get; set; }

        public Boolean SymbolBefore { get; set; }

        public String Format { get; set; }

        public String Fraction { get; set; }

        public Decimal RoundingFactor { get; set; }

        public Int32 ComputationalAccuracy { get; set; }
    }
}