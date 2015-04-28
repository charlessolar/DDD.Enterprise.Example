using FluentValidation;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Tax.Validators
{
    public class AddRegion : AbstractValidator<Commands.AddRegion>
    {
        public AddRegion()
        {
            RuleFor(x => x.TaxId).NotEmpty();
            RuleFor(x => x.RegionId).NotEmpty();
        }
    }
}