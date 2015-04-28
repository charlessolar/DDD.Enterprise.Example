using FluentValidation;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.PaymentOrder.Validators
{
    public class Start : AbstractValidator<Commands.Start>
    {
        public Start()
        {
            RuleFor(x => x.PaymentOrderId).NotEmpty();
            RuleFor(x => x.Identity).NotEmpty().Length(5, Int32.MaxValue);
        }
    }
}