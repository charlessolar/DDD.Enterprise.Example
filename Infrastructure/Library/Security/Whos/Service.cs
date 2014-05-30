using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security.Whos
{
    public class Service : IWho
    {
        public String Description { get { return "Service"; } }
    }
}