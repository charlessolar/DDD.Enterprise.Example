using Demo.Application.ServiceStack.Inventory.Models.Items.Serials;
using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Inventory.Items.Serials.Validators
{
    public class GetValidator : AbstractValidator<Get>
    {
        public GetValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}