using FluentValidation;

namespace Demo.Domain.Inventory.Items.Validators
{
    public class ChangeDescription : AbstractValidator<Commands.ChangeDescription>
    {
        public ChangeDescription()
        {
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.ItemId).NotEmpty();
        }
    }
}