using Demo.Presentation.Inventory.Models.Items.Serials;
using ServiceStack.FluentValidation;

namespace Demo.Presentation.Inventory.Items.Serials.Validators
{
    public class GetValidator : AbstractValidator<Get>
    {
        public GetValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}