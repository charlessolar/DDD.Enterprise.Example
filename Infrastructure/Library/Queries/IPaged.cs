using NServiceBus;
using Demo.Library.Queries.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Queries
{
    public interface IPaged : IQuery
    {
        Int32? Skip { get; set; }
        Int32? Take { get; set; }
        IEnumerable<Sort> Sort { get; set; }
        IEnumerable<Aggregator> Aggregates { get; set; }
        Filter Filter { get; set; }
        Int32? Page { get; set; }
        Int32? PageSize { get; set; }
    }
}
