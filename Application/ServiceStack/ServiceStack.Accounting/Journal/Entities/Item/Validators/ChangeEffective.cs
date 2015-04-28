using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Journal.Entities.Item.Validators
{
    public class ChangeEffective : AbstractValidator<Services.ChangeEffective>
    {
        public ChangeEffective()
        {
            RuleFor(x => x.JournalId).NotEmpty();
            RuleFor(x => x.ItemId).NotEmpty();
            RuleFor(x => x.Effective).NotEmpty();
        }
    }
}