using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Tax.Validators
{
    public class RemoveRegion : AbstractValidator<Services.RemoveRegion>
    {
        public RemoveRegion()
        {
            RuleFor(x => x.TaxId).NotEmpty();
            RuleFor(x => x.RegionId).NotEmpty();
        }
    }
}