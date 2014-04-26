using Domain.Inventory.SerialNumbers.Events;
using Library.Exceptions;
using NES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Inventory.SerialNumbers
{
    public class SerialNumber : AggregateBase
    {
        private Decimal Quantity { get; set; }

        public SerialNumber(Guid SerialNumberId, String SerialNumber, Decimal Quantity, DateTime Effective, Guid ItemId)
        {
            Apply<Created>(e =>
                {
                    e.SerialNumberId = SerialNumberId;
                    e.SerialNumber = SerialNumber;
                    e.Quantity = Quantity;
                    e.Effective = Effective;
                    e.ItemId = ItemId;
                });
        }

        private SerialNumber()
        {
        }

        private void Handle(Created e)
        {
            Id = e.SerialNumberId;
            Quantity = e.Quantity;
        }

        public void TakeQuantity(Decimal Quantity)
        {
            if (this.Quantity < Quantity)
                throw new BusinessLogicException("Can't take more quantity than is on hand");

            this.Quantity -= Quantity;

            Apply<QuantityTaken>(e =>
                {
                    e.SerialNumberId = this.Id;
                    e.Quantity = Quantity;
                });
        }
    }
}