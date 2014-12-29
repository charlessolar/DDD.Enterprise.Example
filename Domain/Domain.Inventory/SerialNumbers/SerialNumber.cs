using Demo.Domain.Inventory.SerialNumbers.Events;
using Demo.Library.Exceptions;
using Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Inventory.SerialNumbers
{
    public class SerialNumber : Aggregate<Guid>
    {
        private SerialNumber() { }

        public void Create(String SerialNumber, Decimal Quantity, DateTime Effective, Guid ItemId)
        {
            Apply<Created>(e =>
            {
                e.SerialNumberId = Id;
                e.SerialNumber = SerialNumber;
                e.Quantity = Quantity;
                e.Effective = Effective;
                e.ItemId = ItemId;
            });
        }
        private void Handle(Created e)
        {
        }

        public void TakeQuantity(Decimal Quantity)
        {
            Apply<QuantityTaken>(e =>
                {
                    e.SerialNumberId = this.Id;
                    e.Quantity = Quantity;
                });
        }

        private void Handle(QuantityTaken e)
        {
        }
    }
}