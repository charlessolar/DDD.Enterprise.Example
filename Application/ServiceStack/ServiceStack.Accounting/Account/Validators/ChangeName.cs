using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Account.Validators
{
    public class ChangeName : AbstractValidator<Services.ChangeName>
    {
        public ChangeName()
        {
            RuleFor(x => x.AccountId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}