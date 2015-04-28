using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Journal.Validators
{
    public class SetCreditAccount : AbstractValidator<Services.SetCreditAccount>
    {
        public SetCreditAccount()
        {
            RuleFor(x => x.JournalId).NotEmpty();
        }
    }
}