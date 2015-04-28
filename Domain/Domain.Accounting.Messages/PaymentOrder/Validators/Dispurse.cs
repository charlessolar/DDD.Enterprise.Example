using FluentValidation;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.PaymentOrder.Validators
{
    public class Dispurse : AbstractValidator<Commands.Dispurse>
    {
        public Dispurse()
        {
            RuleFor(x => x.PaymentOrderId).NotEmpty();
        }
    }
}