using FluentValidation;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.PaymentOrder.Validators
{
    public class Confirm : AbstractValidator<Commands.Confirm>
    {
        public Confirm()
        {
            RuleFor(x => x.PaymentOrderId).NotEmpty();
        }
    }
}