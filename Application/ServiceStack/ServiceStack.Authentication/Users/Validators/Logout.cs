using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Authentication.Users.Validators
{
    public class Logout : AbstractValidator<Services.Logout>
    {
        public Logout()
        {
        }
    }
}