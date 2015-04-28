using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Library.Command;

namespace Demo.Domain.Accounting.Currency.Commands
{
    public class ChangeRoundingFactor : DemoCommand
    {
        public Guid CurrencyId { get; set; }
        public Decimal RoundingFactor { get; set; }
    }
}
