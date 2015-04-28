using Demo.Library.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.Country.Commands
{
    public class UpdateName : DemoCommand
    {
        public Guid CountryId { get; set; }

        public String Name { get; set; }
    }
}