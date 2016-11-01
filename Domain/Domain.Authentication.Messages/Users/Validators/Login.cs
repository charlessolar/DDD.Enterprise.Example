using FluentValidation;

namespace Demo.Domain.Authentication.Users.Validators
{
    public class Login : AbstractValidator<Commands.Login>
    {
        public Login()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.NickName).NotEmpty();
            RuleFor(x => x.ImageType).NotEmpty();
            RuleFor(x => x.ImageData).NotEmpty();
        }
    }
}