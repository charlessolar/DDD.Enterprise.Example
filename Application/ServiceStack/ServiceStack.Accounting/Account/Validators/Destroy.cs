using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Account.Validators
{
    public class Destroy : AbstractValidator<Services.Destroy>
    {
        public Destroy()
        {
            RuleFor(x => x.AccountId).NotEmpty();
        }
    }
}