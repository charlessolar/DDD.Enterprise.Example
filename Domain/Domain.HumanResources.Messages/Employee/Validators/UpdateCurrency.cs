using FluentValidation;
using Demo.Library.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.HumanResources.Employee.Validators
{
    public class UpdateCurrency : AbstractValidator<Commands.UpdateCurrency>
    {
        public UpdateCurrency()
        {
            RuleFor(x => x.EmployeeId).NotEmpty();
            RuleFor(x => x.CurrencyId).NotEmpty();
        }
    }
}