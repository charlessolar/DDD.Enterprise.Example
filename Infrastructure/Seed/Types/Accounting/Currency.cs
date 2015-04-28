using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.Types.Accounting
{
    public class Currency
    {
        public Guid Id { get; set; }

        public String Code { get; set; }

        public String Name { get; set; }

        public String Symbol { get; set; }

        public Boolean SymbolBefore { get; set; }

        public Decimal RoundingFactor { get; set; }

        public Int32 ComputationalAccuracy { get; set; }

        public Boolean Activated { get; set; }

        public String Format { get; set; }

        public String Fraction { get; set; }
    }
}