using Demo.Application.ServiceStack.Inventory.Models.Items;
using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Inventory.Items.Validators
{
    public class GetValidator : AbstractValidator<Get>
    {
        public GetValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}