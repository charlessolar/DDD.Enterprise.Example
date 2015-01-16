
using Forte.Application.ServiceStack.Authentication.Models.Users;
using ServiceStack.FluentValidation;

namespace Forte.Presentation.Authentication.Users.Validators
{
    public class ChangeEmailValidator : AbstractValidator<ChangeEmail>
    {
        public ChangeEmailValidator()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
        }
    }
}