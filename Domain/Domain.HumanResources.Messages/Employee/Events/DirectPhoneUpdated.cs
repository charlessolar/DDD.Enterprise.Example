using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.HumanResources.Employee.Events
{
    public interface DirectPhoneUpdated : IEvent
    {
        Guid EmployeeId { get; set; }

        String Phone { get; set; }
    }
}