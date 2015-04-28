using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.Region.Entities.Match
{
    public class Match : Aggregates.Entity<Guid>, IMatch
    {
        private Match()
        {
        }

        public void Create(String Value, MATCH_TYPE Type, MATCH_OPERATION Operation)
        {
            Apply<Events.Created>(e =>
            {
                e.RegionId = AggregateId;
                e.MatchId = Id;
                e.Value = Value;
                e.Type = Type;
                e.Operation = Operation;
            });
        }

        public void ChangeValue(String Value)
        {
            Apply<Events.ValueChanged>(e =>
            {
                e.RegionId = AggregateId;
                e.MatchId = Id;
                e.Value = Value;
            });
        }

        public void ChangeType(MATCH_TYPE Type)
        {
            Apply<Events.TypeChanged>(e =>
            {
                e.RegionId = AggregateId;
                e.MatchId = Id;
                e.Type = Type;
            });
        }

        public void ChangeOperation(MATCH_OPERATION Operation)
        {
            Apply<Events.OperationChanged>(e =>
            {
                e.RegionId = AggregateId;
                e.MatchId = Id;
                e.Operation = Operation;
            });
        }

        public void Destroy()
        {
            Apply<Events.Destroyed>(e =>
            {
                e.RegionId = AggregateId;
                e.MatchId = Id;
            });
        }
    }
}