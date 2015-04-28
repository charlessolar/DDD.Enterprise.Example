using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.PaymentOrder.Validators
{
    public class ChangeMethod : AbstractValidator<Services.ChangeMethod>
    {
        public ChangeMethod()
        {
            RuleFor(x => x.PaymentOrderId).NotEmpty();
            RuleFor(x => x.MethodId).NotEmpty();
        }
    }
}