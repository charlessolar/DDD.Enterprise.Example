using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Authentication.Users.Validators
{
    public class ChangeEmail : AbstractValidator<Services.ChangeEmail>
    {
        public ChangeEmail()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
        }
    }
}