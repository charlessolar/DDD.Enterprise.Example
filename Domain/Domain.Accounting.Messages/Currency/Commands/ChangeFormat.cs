using Demo.Library.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Currency.Commands
{
    public class ChangeFormat : DemoCommand
    {
        public Guid CurrencyId { get; set; }

        public String Format { get; set; }
    }
}