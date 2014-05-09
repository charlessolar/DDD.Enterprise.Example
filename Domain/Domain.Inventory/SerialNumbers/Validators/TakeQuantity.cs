using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Inventory.SerialNumbers.Validators
{
    public class TakeQuantity : AbstractValidator<Commands.TakeQuantity>
    {
        public TakeQuantity()
        {
            RuleFor(x => x.SerialNumberId).NotEmpty();
            RuleFor(x => x.Quantity).GreaterThan(0);
        }
    }
}