using FluentValidation;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.PaymentOrder.Validators
{
    public class ChangeEffective : AbstractValidator<Commands.ChangeEffective>
    {
        public ChangeEffective()
        {
            RuleFor(x => x.PaymentOrderId).NotEmpty();
            RuleFor(x => x.Effective).NotEmpty();
        }
    }
}