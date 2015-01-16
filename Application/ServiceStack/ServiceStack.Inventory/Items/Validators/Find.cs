using Forte.Application.ServiceStack.Inventory.Models.Items;
using ServiceStack.FluentValidation;

namespace Forte.Application.ServiceStack.Inventory.Items.Validators
{
    public class FindValidator : AbstractValidator<Find>
    {
        public FindValidator()
            : base()
        {
        }
    }
}