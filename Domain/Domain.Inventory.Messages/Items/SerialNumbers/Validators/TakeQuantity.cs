using FluentValidation;

namespace Demo.Domain.Inventory.Items.SerialNumbers.Validators
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