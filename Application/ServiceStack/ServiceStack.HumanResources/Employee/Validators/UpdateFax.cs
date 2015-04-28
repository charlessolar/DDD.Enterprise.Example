using Demo.Library.Command;
using Demo.Library.Responses;
using ServiceStack;
using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.HumanResources.Employee.Validators
{
    public class UpdateFax : AbstractValidator<Services.UpdateFax>
    {
        public UpdateFax()
        {
            RuleFor(x => x.EmployeeId).NotEmpty();
        }
    }
}