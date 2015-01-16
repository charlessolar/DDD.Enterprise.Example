using Demo.Application.ServiceStack.Inventory.Models.Items;
using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Inventory.Items.Validators
{
    public class FindValidator : AbstractValidator<Find>
    {
        public FindValidator()
            : base()
        {
        }
    }
}