using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.PaymentOrder.Validators
{
    public class Start : AbstractValidator<Services.Start>
    {
        public Start()
        {
            RuleFor(x => x.PaymentOrderId).NotEmpty();
            RuleFor(x => x.Identity).NotEmpty();
        }
    }
}