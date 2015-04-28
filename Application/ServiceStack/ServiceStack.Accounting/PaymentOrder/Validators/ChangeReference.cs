using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.PaymentOrder.Validators
{
    public class ChangeReference : AbstractValidator<Services.ChangeReference>
    {
        public ChangeReference()
        {
            RuleFor(x => x.PaymentOrderId).NotEmpty();
            RuleFor(x => x.Reference).NotEmpty();
        }
    }
}