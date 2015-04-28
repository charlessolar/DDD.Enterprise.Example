using Demo.Library.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Currency.Commands
{
    public class AddRate : DemoCommand
    {
        public Guid CurrencyId { get; set; }

        public Guid DestinationCurrencyId { get; set; }


        public Decimal Factor { get; set; }

        public DateTime? EffectiveTill { get; set; }
    }
}