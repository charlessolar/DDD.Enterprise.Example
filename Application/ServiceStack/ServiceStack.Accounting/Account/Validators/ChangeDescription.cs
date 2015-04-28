using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Account.Validators
{
    public class ChangeDescription : AbstractValidator<Services.ChangeDescription>
    {
        public ChangeDescription()
        {
            RuleFor(x => x.AccountId).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}