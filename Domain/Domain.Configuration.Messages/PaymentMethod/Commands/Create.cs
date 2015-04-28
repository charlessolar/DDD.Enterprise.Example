using Demo.Library.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.PaymentMethod.Commands
{
    public class Create : DemoCommand
    {
        public Guid PaymentMethodId { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public Guid? ParentId { get; set; }
    }
}