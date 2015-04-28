using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Journal.Entities.Item.Validators
{
    public class ChangeReference : AbstractValidator<Services.ChangeReference>
    {
        public ChangeReference()
        {
            RuleFor(x => x.JournalId).NotEmpty();
            RuleFor(x => x.ItemId).NotEmpty();
            RuleFor(x => x.Reference).NotEmpty();
        }
    }
}