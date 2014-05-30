using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security.Policies
{
    public class Default : IPolicy
    {
        public String Description { get; private set; }

        public ICollection<Tuple<IRule, Effect>> Rules { get; private set; }

        public ICollection<MatchDefinition> Whos { get; private set; }
        public ICollection<MatchDefinition> Whats { get; private set; }
        public ICollection<MatchDefinition> Hows { get; private set; }

        public Default()
        {
            Description = "Default";
            Whos = new List<MatchDefinition>();
            Whats = new List<MatchDefinition>();
            Hows = new List<MatchDefinition>();
            Rules = new List<Tuple<IRule, Effect>>();
        }

        public Boolean Matches(IRequest request)
        {
            return true;
        }

        public Effect Resolve(IRequest request)
        {
            var rule = Rules.FirstOrDefault(r => r.Item1.Matches(request));
            return rule.Item2;
        }
    }
}