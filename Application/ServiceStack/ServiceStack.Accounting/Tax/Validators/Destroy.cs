using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Tax.Validators
{
    public class Destroy : AbstractValidator<Services.Destroy>
    {
        public Destroy()
        {
            RuleFor(x => x.TaxId).NotEmpty();
        }
    }
}