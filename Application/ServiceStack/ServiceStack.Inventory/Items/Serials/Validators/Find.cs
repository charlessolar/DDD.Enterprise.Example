using Demo.Application.ServiceStack.Inventory.Models.Items.Serials;
using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Inventory.Items.Serials.Validators
{
    public class FindValidator : AbstractValidator<Find>
    {
        public FindValidator()
            : base()
        {
        }
    }
}