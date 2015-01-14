using Demo.Presentation.Inventory.Models.Items;
using ServiceStack.FluentValidation;

namespace Demo.Presentation.Inventory.Items.Validators
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