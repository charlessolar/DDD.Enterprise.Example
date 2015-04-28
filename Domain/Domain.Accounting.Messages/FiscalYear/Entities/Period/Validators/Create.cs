using FluentValidation;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.FiscalYear.Entities.Period.Validators
{
    public class Create : AbstractValidator<Commands.Create>
    {
        public Create()
        {
            RuleFor(x => x.FiscalYearId).NotEmpty();
            RuleFor(x => x.PeriodId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Code).NotEmpty().Length(2, 8);
        }
    }
}