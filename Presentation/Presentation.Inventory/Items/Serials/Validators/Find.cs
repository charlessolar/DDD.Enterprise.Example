using Demo.Presentation.Inventory.Models.Items.Serials;
using ServiceStack.FluentValidation;

namespace Demo.Presentation.Inventory.Items.Serials.Validators
{
    public class FindValidator : AbstractValidator<Find>
    {
        public FindValidator()
            : base()
        {
        }
    }
}