using FluentValidation;

namespace Demo.Domain.Inventory.Items.SerialNumbers.Validators
{
    public class Create : AbstractValidator<Commands.Create>
    {
        public Create()
        {
            RuleFor(x => x.SerialNumber).NotEmpty().Length(2, 32).WithMessage("Serial must be between 2 and 32 characters");
            RuleFor(x => x.Effective).NotEmpty();
            RuleFor(x => x.Quantity).NotEmpty().GreaterThan(0.0M);
            RuleFor(x => x.ItemId).NotEmpty();
        }
    }
}