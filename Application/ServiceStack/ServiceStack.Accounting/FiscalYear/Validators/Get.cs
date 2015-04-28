using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Accounting.FiscalYear.Validators
{
    public class Get : AbstractValidator<Services.Get>
    {
        public Get()
        {
            RuleFor(x => x.FiscalYearId).NotEmpty();
        }
    }
}
