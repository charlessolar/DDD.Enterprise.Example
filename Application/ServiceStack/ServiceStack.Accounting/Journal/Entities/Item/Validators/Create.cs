using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Journal.Entities.Item.Validators
{
    public class Create : AbstractValidator<Services.Create>
    {
        public Create()
        {
            RuleFor(x => x.JournalId).NotEmpty();
            RuleFor(x => x.ItemId).NotEmpty();
            RuleFor(x => x.Effective).NotEmpty();
            RuleFor(x => x.Reference).NotEmpty();
            RuleFor(x => x.AccountId).NotEmpty();
            RuleFor(x => x.PeriodId).NotEmpty();
            RuleFor(x => x.Amount).NotEqual(0);
        }
    }
}