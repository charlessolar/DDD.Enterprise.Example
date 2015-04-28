using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.HumanResources.Employee.Validators
{
    public class UnlinkUser : AbstractValidator<Services.UnlinkUser>
    {
        public UnlinkUser()
        {
            RuleFor(x => x.EmployeeId).NotEmpty();
        }
    }
}