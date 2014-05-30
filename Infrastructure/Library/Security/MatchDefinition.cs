using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security
{
    public class MatchDefinition
    {
        public String Operation { get; set; }

        public String Name { get; set; }
        public String Value { get; set; }

        public Boolean MustBePresent { get; set; }
    }
}