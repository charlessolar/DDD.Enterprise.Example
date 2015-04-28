using FluentValidation;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.HumanResources.Employee.Validators
{
    public class UpdateGender : AbstractValidator<Commands.UpdateGender>
    {
        public UpdateGender()
        {
            RuleFor(x => x.EmployeeId).NotEmpty();
            RuleFor(x => x.Gender).NotNull();
        }
    }
}