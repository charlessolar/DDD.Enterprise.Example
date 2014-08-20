using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.Inventory.SerialNumbers.Validators
{
    public class CreateSerialNumberValidator : AbstractValidator<CreateSerialNumber>
    {
        public CreateSerialNumberValidator()
        {
            RuleFor(x => x.SerialNumber).NotEmpty().Length(2, 32).WithMessage("Serial must be between 2 and 32 characters");
            RuleFor(x => x.Effective).NotEmpty();
            RuleFor(x => x.Quantity).NotEmpty().GreaterThan(0.0M);
            RuleFor(x => x.ItemId).NotEmpty();
        }
    }
}