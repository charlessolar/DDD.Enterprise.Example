using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Journal.Validators
{
    public class SetDebitAccount : AbstractValidator<Services.SetDebitAccount>
    {
        public SetDebitAccount()
        {
            RuleFor(x => x.JournalId).NotEmpty();
        }
    }
}