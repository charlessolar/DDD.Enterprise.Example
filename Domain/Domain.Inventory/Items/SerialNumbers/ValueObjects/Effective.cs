using Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Inventory.Items.SerialNumbers.ValueObjects
{
    public class Effective : ValueObject<Effective>
    {
        public readonly DateTime Date;

        public Effective(DateTime Effective)
        {
            this.Date = Effective;
        }
    }
}
