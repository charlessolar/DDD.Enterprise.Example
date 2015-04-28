using Demo.Library.Command;
using Demo.Library.Responses;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.HumanResources.Employee.Services
{
    [Api("HumanResources")]
    [Route("/human-resources/employee/{EmployeeId}/national-id", "PUT POST")]
    public class UpdateNationalId : IReturn<Base<Command>>
    {
        public Guid EmployeeId { get; set; }

        public String NationalId { get; set; }
    }
}