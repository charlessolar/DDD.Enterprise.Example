using Demo.Application.ServiceStack.Authentication.Models.Users;
using ServiceStack.FluentValidation;

namespace Demo.Presentation.Authentication.Users.Validators
{
    public class LogoutValidator : AbstractValidator<Logout>
    {
        public LogoutValidator()
        {
        }
    }
}