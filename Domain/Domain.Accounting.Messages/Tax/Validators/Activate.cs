using FluentValidation;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Tax.Validators
{
    public class Activate : AbstractValidator<Commands.Activate>
    {
        public Activate()
        {
            RuleFor(x => x.TaxId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}