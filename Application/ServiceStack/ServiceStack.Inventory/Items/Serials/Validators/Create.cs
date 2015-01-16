using Forte.Application.ServiceStack.Inventory.Models.Items.Serials;
using ServiceStack.FluentValidation;

namespace Forte.Application.ServiceStack.Inventory.Items.Serials.Validators
{
    public class CreateValidator : AbstractValidator<Create>
    {
        public CreateValidator()
        {
            RuleFor(x => x.SerialNumber).NotEmpty().Length(2, 32).WithMessage("Serial must be between 2 and 32 characters");
            RuleFor(x => x.Effective).NotEmpty();
            RuleFor(x => x.Quantity).NotEmpty().GreaterThan(0.0M);
            RuleFor(x => x.ItemId).NotEmpty();
        }
    }
}