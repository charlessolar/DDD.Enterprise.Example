using FluentValidation;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.FiscalYear.Entities.Period.Validators
{
    public class Start : AbstractValidator<Commands.Start>
    {
        public Start()
        {
            RuleFor(x => x.FiscalYearId).NotEmpty();
            RuleFor(x => x.PeriodId).NotEmpty();
            RuleFor(x => x.Effective).NotEmpty();
        }
    }
}