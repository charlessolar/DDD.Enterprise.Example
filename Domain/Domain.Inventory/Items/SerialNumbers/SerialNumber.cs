using Demo.Domain.Inventory.Items.SerialNumbers.Events;
using Demo.Library.Exceptions;
using Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Inventory.Items.SerialNumbers
{
    public class SerialNumber : Entity<Guid>
    {
        public ValueObjects.SerialNumber Serial { get; private set; }
        public ValueObjects.Quantity Quantity { get; private set; }
        public ValueObjects.Effective Effective { get; private set; }
        public Guid ItemId { get; private set; }

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
            this.Serial = new ValueObjects.SerialNumber(e.SerialNumber);
            this.Quantity = new ValueObjects.Quantity(e.Quantity);
            this.Effective = new ValueObjects.Effective(e.Effective);
            this.ItemId = e.ItemId;
        }

        public void TakeQuantity(Decimal Quantity)
        {
            var newQuantity = new ValueObjects.Quantity(this.Quantity.Total - Quantity);

            var spec = new Specifications.Quantity { MinValue = 0 };
            if (!spec.IsSatisfiedBy(newQuantity))
                throw new BusinessLogicException("Can't take more quantity than is on hand");

            Apply<QuantityTaken>(e =>
                {
                    e.SerialNumberId = this.Id;
                    e.Quantity = Quantity;
                });
        }

        private void Handle(QuantityTaken @event)
        {
            this.Quantity = new ValueObjects.Quantity(this.Quantity.Total - @event.Quantity);
        }
    }
}