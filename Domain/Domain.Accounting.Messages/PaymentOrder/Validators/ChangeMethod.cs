using FluentValidation;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.PaymentOrder.Validators
{
    public class ChangeMethod : AbstractValidator<Commands.ChangeMethod>
    {
        public ChangeMethod()
        {
            RuleFor(x => x.PaymentOrderId).NotEmpty();
            RuleFor(x => x.MethodId).NotEmpty();
        }
    }
}