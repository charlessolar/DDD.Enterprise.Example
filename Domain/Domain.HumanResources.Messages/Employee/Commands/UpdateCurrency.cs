using Demo.Library.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.HumanResources.Employee.Commands
{
    public class UpdateCurrency : DemoCommand
    {
        public Guid EmployeeId { get; set; }

        public Guid CurrencyId { get; set; }
    }
}