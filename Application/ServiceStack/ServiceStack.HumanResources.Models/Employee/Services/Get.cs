using Demo.Library.Queries;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.HumanResources.Employee.Services
{
    [Api("HumanResources")]
    [Route("/human-resources/employee/{EmployeeId}", "GET")]
    public class Get : Query<Responses.Get>
    {
        public Guid EmployeeId { get; set; }
    }
}