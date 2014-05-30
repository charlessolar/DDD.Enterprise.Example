using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security
{
    public interface IRule
    {
        String Description { get; }

        ICollection<MatchDefinition> Whos { get; }
        ICollection<MatchDefinition> Whats { get; }
        ICollection<MatchDefinition> Hows { get; }

        Boolean Matches(IRequest request);
    }
}