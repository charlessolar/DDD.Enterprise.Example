using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.Region.Entities.Match
{
    public interface IMatch : Aggregates.Contracts.IEventSource<Guid>
    {
    }
}