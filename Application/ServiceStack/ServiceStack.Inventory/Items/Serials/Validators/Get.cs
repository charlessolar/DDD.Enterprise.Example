using Forte.Application.ServiceStack.Inventory.Models.Items.Serials;
using ServiceStack.FluentValidation;

namespace Forte.Application.ServiceStack.Inventory.Items.Serials.Validators
{
    public class GetValidator : AbstractValidator<Get>
    {
        public GetValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}