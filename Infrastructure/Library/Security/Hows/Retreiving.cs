using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security.Hows
{
    // Happens when pulling data from something - used for row-level filtering
    public class Retreiving : IHow
    {
        public String Description { get; set; }
        private IList<IWhat> _whats;

        public Retreiving()
        {
            _whats = new List<IWhat>();
        }

        public void AddWhat(IWhat what)
        {
            _whats.Add(what);
        }
    }
}