using Demo.Library.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.PaymentMethod.Entities.Detail.Commands
{
    public class Create : DemoCommand
    {
        public Guid PaymentMethodId { get; set; }

        public Guid DetailId { get; set; }

        public String Name { get; set; }

        public String Hint { get; set; }
    }
}