using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.PaymentOrder.Validators
{
    public class Confirm : AbstractValidator<Services.Confirm>
    {
        public Confirm()
        {
            RuleFor(x => x.PaymentOrderId).NotEmpty();
        }
    }
}