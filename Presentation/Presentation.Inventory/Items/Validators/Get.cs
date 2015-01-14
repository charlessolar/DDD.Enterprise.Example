using Demo.Presentation.Inventory.Models.Items;
using ServiceStack.FluentValidation;

namespace Demo.Presentation.Inventory.Items.Validators
{
    public class GetValidator : AbstractValidator<Get>
    {
        public GetValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}