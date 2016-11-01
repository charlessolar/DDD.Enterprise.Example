using Demo.Library.Queries.Grid;
using System.Collections.Generic;

namespace Demo.Library.Queries
{
    public interface IPaged : IQuery
    {
        int? Skip { get; set; }
        int? Take { get; set; }
        IEnumerable<Sort> Sort { get; set; }
        IEnumerable<Aggregator> Aggregates { get; set; }
        Filter Filter { get; set; }
        int? Page { get; set; }
        int? PageSize { get; set; }
    }
}
