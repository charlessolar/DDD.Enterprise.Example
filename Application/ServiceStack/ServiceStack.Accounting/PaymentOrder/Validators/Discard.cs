using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.PaymentOrder.Validators
{
    public class Discard : AbstractValidator<Services.Discard>
    {
        public Discard()
        {
            RuleFor(x => x.PaymentOrderId).NotEmpty();
        }
    }
}