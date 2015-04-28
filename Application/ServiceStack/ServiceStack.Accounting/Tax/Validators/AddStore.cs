using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Tax.Validators
{
    public class AddStore : AbstractValidator<Services.AddStore>
    {
        public AddStore()
        {
            RuleFor(x => x.TaxId).NotEmpty();
            RuleFor(x => x.StoreId).NotEmpty();
        }
    }
}