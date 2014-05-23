using Demo.Domain.Inventory.SerialNumbers.Events;
using Demo.Library.Exceptions;
using Demo.Library.Identity;
using NES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Inventory.SerialNumbers
{
    public class SerialNumber : IdentityAggregateRoot<Identities.SerialNumber>
    {
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

            _identity = new Identities.SerialNumber
            {
                Id = e.SerialNumberId,
                Serial = e.SerialNumber,
                Quantity = e.Quantity,
                Effective = e.Effective,
                ItemId = e.ItemId,
            };
        }

        public void TakeQuantity(Decimal Quantity)
        {
            if (_identity.Quantity < Quantity)
                throw new BusinessLogicException("Can't take more quantity than is on hand");


            Apply<QuantityTaken>(e =>
                {
                    e.SerialNumberId = this.Id;
                    e.Quantity = Quantity;
                });
        }

        public void Handle(QuantityTaken e)
        {
            _identity.Quantity -= e.Quantity;
        }
    }
}