using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Journal.Entities.Item.Validators
{
    public class Reconcile : AbstractValidator<Services.Reconcile>
    {
        public Reconcile()
        {
            RuleFor(x => x.JournalId).NotEmpty();
            RuleFor(x => x.ItemId).NotEmpty();
            RuleFor(x => x.OtherItemId).NotEmpty();
            RuleFor(x => x.Amount).GreaterThanOrEqualTo(0);
        }
    }
}