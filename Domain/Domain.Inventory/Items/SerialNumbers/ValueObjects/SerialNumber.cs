using Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Inventory.Items.SerialNumbers.ValueObjects
{
    public class SerialNumber : ValueObject<SerialNumber>
    {
        public readonly String Serial;

        public SerialNumber(String SerialNumber)
        {
            this.Serial = SerialNumber;
        }
    }
}
