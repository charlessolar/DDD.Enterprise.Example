using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security
{
    public class Context : IContext
    {
        public ICollection<IWho> Whos { get; private set; }
        public ICollection<IHow> Hows { get; private set; }
        public ICollection<IWhat> Whats { get; private set; }

        public Context()
        {
            Whos = new List<IWho>();
            Hows = new List<IHow>();
            Whats = new List<IWhat>();
        }
    }
}