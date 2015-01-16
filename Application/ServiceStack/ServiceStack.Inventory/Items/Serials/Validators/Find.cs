using Forte.Application.ServiceStack.Inventory.Models.Items.Serials;
using ServiceStack.FluentValidation;

namespace Forte.Application.ServiceStack.Inventory.Items.Serials.Validators
{
    public class FindValidator : AbstractValidator<Find>
    {
        public FindValidator()
            : base()
        {
        }
    }
}