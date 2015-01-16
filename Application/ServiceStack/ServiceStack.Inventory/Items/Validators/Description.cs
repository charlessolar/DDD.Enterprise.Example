using Forte.Application.ServiceStack.Inventory.Models.Items;
using ServiceStack.FluentValidation;

namespace Forte.Application.ServiceStack.Inventory.Items.Validators
{
    public class DescriptionValidator : AbstractValidator<Description>
    {
        public DescriptionValidator()
        {
            RuleFor(x => x.ItemId).NotEmpty();
            RuleFor(x => x.Data).NotEmpty();
        }
    }
}