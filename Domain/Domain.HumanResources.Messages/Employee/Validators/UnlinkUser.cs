using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.HumanResources.Employee.Validators
{
    public class UnlinkUser : AbstractValidator<Commands.UnlinkUser>
    {
        public UnlinkUser()
        {
            RuleFor(x => x.EmployeeId).NotEmpty();
        }
    }
}