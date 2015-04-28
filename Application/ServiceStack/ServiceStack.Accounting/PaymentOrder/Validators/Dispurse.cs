using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.PaymentOrder.Validators
{
    public class Dispurse : AbstractValidator<Services.Dispurse>
    {
        public Dispurse()
        {
            RuleFor(x => x.PaymentOrderId).NotEmpty();
        }
    }
}