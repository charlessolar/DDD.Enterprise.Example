using Demo.Presentation.Authentication.Models.Users;
using ServiceStack.FluentValidation;

namespace Demo.Presentation.Authentication.Users.Validators
{
    public class LoginValidator : AbstractValidator<Login>
    {
        public LoginValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.NickName).NotEmpty();
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
            RuleFor(x => x.ImageUrl).NotEmpty();
        }
    }
}