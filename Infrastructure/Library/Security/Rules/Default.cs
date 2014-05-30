using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security.Rules
{
    public class Default : IRule
    {
        public String Description { get; private set; }

        public ICollection<MatchDefinition> Whos { get; private set; }
        public ICollection<MatchDefinition> Whats { get; private set; }
        public ICollection<MatchDefinition> Hows { get; private set; }

        public Default()
        {
            Description = "Default";
            Whos = new List<MatchDefinition>();
            Whats = new List<MatchDefinition>();
            Hows = new List<MatchDefinition>();
        }

        public Boolean Matches(IRequest request)
        {
            if (!Whos.Any(w => w.Name == request.Who.Item1 && w.Value == request.Who.Item2))
                return false;
            if (!Whats.Any(w => w.Name == request.What.Item1 && w.Value == request.What.Item2))
                return false;
            if (!Hows.Any(w => w.Name == request.How.Item1 && w.Value == request.How.Item2))
                return false;
            return true;
        }
    }
}