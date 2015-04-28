using FluentValidation;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.FiscalYear.Entities.Period.Validators
{
    public class End : AbstractValidator<Commands.End>
    {
        public End()
        {
            RuleFor(x => x.FiscalYearId).NotEmpty();
            RuleFor(x => x.PeriodId).NotEmpty();
            RuleFor(x => x.Effective).NotEmpty();
        }
    }
}