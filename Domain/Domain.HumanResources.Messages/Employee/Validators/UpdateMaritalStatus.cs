using FluentValidation;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.HumanResources.Employee.Validators
{
    public class UpdateMaritalStatus : AbstractValidator<Commands.UpdateMaritalStatus>
    {
        public UpdateMaritalStatus()
        {
            RuleFor(x => x.EmployeeId).NotEmpty();
            RuleFor(x => x.MaritalStatus).NotNull();
        }
    }
}