
using Forte.Application.ServiceStack.Authentication.Models.Users;
using ServiceStack.FluentValidation;

namespace Forte.Presentation.Authentication.Users.Validators
{
    public class GetValidator : AbstractValidator<Get>
    {
        public GetValidator()
        {
        }
    }
}