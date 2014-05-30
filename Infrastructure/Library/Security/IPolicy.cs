using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security
{
    public enum Effect
    {
        ALLOW,
        DENY
    }

    public interface IPolicy
    {
        String Description { get; }

        ICollection<Tuple<IRule, Effect>> Rules { get; }

        ICollection<MatchDefinition> Whos { get; }
        ICollection<MatchDefinition> Whats { get; }
        ICollection<MatchDefinition> Hows { get; }

        Boolean Matches(IRequest request);
        Effect Resolve(IRequest request);
    }
}