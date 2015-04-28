using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Tax.Validators
{
    public class AddRegion : AbstractValidator<Services.AddRegion>
    {
        public AddRegion()
        {
            RuleFor(x => x.TaxId).NotEmpty();
            RuleFor(x => x.RegionId).NotEmpty();
        }
    }
}