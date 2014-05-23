using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security
{
    public class Context
    {
        public IWho Who { get; set; }
        public IHow How { get; set; }
        public IWhat What { get; set; }
    }
}