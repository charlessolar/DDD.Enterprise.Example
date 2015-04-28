using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Tax.Validators
{
    public class RemoveStore : AbstractValidator<Services.RemoveStore>
    {
        public RemoveStore()
        {
            RuleFor(x => x.TaxId).NotEmpty();
            RuleFor(x => x.StoreId).NotEmpty();
        }
    }
}