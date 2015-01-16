
using Demo.Application.ServiceStack.Authentication.Models.Users;
using ServiceStack.FluentValidation;

namespace Demo.Presentation.Authentication.Users.Validators
{
    public class ChangeEmailValidator : AbstractValidator<ChangeEmail>
    {
        public ChangeEmailValidator()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
        }
    }
}