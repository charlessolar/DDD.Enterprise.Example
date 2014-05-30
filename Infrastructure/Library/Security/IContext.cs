using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security
{
    public interface IContext
    {
        ICollection<IWho> Whos { get; }
        ICollection<IHow> Hows { get; }
        ICollection<IWhat> Whats { get; }
    }
}