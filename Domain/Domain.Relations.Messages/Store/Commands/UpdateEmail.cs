using Demo.Library.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Relations.Store.Commands
{
    public class UpdateEmail : DemoCommand
    {
        public Guid StoreId { get; set; }

        public String Email { get; set; }
    }
}