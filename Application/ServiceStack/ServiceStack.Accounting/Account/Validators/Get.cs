using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Account.Validators
{
    public class Get : AbstractValidator<Services.Get>
    {
        public Get()
        {
            RuleFor(x => x.AccountId).NotEmpty();
        }
    }
}