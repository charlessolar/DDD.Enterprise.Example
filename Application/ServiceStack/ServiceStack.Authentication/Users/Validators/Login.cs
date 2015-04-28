using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Authentication.Users.Validators
{
    public class Login : AbstractValidator<Services.Login>
    {
        public Login()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.NickName).NotEmpty();
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
            RuleFor(x => x.ImageUrl).NotEmpty();
        }
    }
}