using Demo.Presentation.Inventory.Models.Items;
using ServiceStack.FluentValidation;

namespace Demo.Presentation.Inventory.Items.Validators
{
    public class FindValidator : AbstractValidator<Find>
    {
        public FindValidator()
            : base()
        {
        }
    }
}