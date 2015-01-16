
using Demo.Application.ServiceStack.Authentication.Models.Users;
using ServiceStack.FluentValidation;

namespace Demo.Presentation.Authentication.Users.Validators
{
    public class GetValidator : AbstractValidator<Get>
    {
        public GetValidator()
        {
        }
    }
}