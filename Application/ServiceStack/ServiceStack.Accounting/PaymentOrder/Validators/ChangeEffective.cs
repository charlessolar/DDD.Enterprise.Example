using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.PaymentOrder.Validators
{
    public class ChangeEffective : AbstractValidator<Services.ChangeEffective>
    {
        public ChangeEffective()
        {
            RuleFor(x => x.PaymentOrderId).NotEmpty();
            RuleFor(x => x.Effective).NotEmpty();
        }
    }
}