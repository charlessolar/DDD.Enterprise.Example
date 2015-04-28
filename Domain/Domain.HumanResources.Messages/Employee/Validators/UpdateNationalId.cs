using FluentValidation;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.HumanResources.Employee.Validators
{
    public class UpdateNationalId : AbstractValidator<Commands.UpdateNationalId>
    {
        public UpdateNationalId()
        {
            RuleFor(x => x.EmployeeId).NotEmpty();
        }
    }
}