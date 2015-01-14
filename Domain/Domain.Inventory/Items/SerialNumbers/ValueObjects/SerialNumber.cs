using Aggregates;
using System;

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