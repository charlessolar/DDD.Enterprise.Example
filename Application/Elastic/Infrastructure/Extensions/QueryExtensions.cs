using Nest;
using Demo.Library.Queries;
using Demo.Library.Queries.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.Elastic.Infrastructure.Extensions
{
    public static class QueryExtensions
    {
        public static SearchDescriptor<T> Paging<T>(this SearchDescriptor<T> descriptor, IPaged request) where T : class
        {
            if (request.Skip.HasValue)
                descriptor = descriptor.Skip(request.Skip.Value);
            if(request.Take.HasValue)
                descriptor = descriptor.Take(request.Take.Value);


            if (request.Filter != null)
                descriptor = descriptor.PostFilter(f => f.Grid(request.Filter));
            if (request.Sort != null && request.Sort.Any())
                descriptor = descriptor.Sort(request.Sort);

            return descriptor;
        }
        public static SearchDescriptor<T> Sort<T>(this SearchDescriptor<T> descriptor, IEnumerable<Library.Queries.Grid.Sort> sorts) where T : class
        {
            foreach (var sort in sorts)
            {
                if (sort.Dir == "desc")
                    descriptor = descriptor.Sort(f => f.Field(sort.Field, SortOrder.Descending));
                if (sort.Dir == "asc")
                    descriptor = descriptor.Sort(f => f.Field(sort.Field, SortOrder.Ascending));
            }
            return descriptor;
        }

        public static QueryContainer Grid<T>(this QueryContainerDescriptor<T> descriptor, Filter filter) where T : class
        {
            if (filter.Value != null)
                filter.Value = filter.Value.ToLowerInvariant();

            var trues = new List<QueryContainer>();
            var falses = new List<QueryContainer>();
            var should = new List<QueryContainer>();

            switch (filter.Operator)
            {
                case "eq":
                    trues.Add(descriptor.Term(filter.Field, filter.Value));
                    break;

                case "neq":
                    falses.Add(descriptor.Term(filter.Field, filter.Value));
                    break;

                case "lt":
                    {
                        Double value;
                        if (Double.TryParse(filter.Value, out value))
                            trues.Add(descriptor.Range(f => f.Field(filter.Field).LessThan(value)));
                        break;
                    }
                case "lte":
                    {
                        Double value;
                        if (Double.TryParse(filter.Value, out value))
                            trues.Add(descriptor.Range(f => f.Field(filter.Field).LessThanOrEquals(value)));
                        break;
                    }
                case "gt":
                    {
                        Double value;
                        if (Double.TryParse(filter.Value, out value))
                            trues.Add(descriptor.Range(f => f.Field(filter.Field).GreaterThan(value)));
                        break;
                    }
                case "gte":
                    {
                        Double value;
                        if (Double.TryParse(filter.Value, out value))
                            trues.Add(descriptor.Range(f => f.Field(filter.Field).GreaterThanOrEquals(value)));
                        break;
                    }
                case "startswith":
                    trues.Add(descriptor.Prefix(filter.Field, filter.Value));
                    break;

                case "endswith":
                    trues.Add(descriptor.MatchPhrasePrefix(mpp => mpp.Analyzer("suffix").Field(filter.Field).Query(filter.Value)));
                    break;

                case "contains":
                    trues.Add(descriptor.Term(filter.Field, filter.Value));
                    break;

                case "doesnotcontain":
                    falses.Add(descriptor.Term(filter.Field, filter.Value));
                    break;
            }

            // Special condition for range filter
            if (filter.Logic == "and" && filter.Filters != null && filter.Filters.Count() == 2 && filter.Filters.All(f => new[] { "lt", "lte", "gt", "gte" }.Contains(f.Operator)) && !filter.Filters.GroupBy(f => f.Field).Skip(1).Any())
            {
                var field = filter.Filters.First().Field;
                // Range query
                trues.Add(descriptor.Range(f => f.Field(field).Filter(filter.Filters.First(), filter.Filters.Last())));
            }
            else if (filter.Filters != null && filter.Filters.Any())
            {
                foreach (var child in filter.Filters)
                    trues.Add(descriptor.Grid(child));
            }

            if (!String.IsNullOrEmpty(filter.Logic) && filter.Logic.ToLower() == "or")
                return descriptor.Bool(f => f.Should(trues.ToArray()).MustNot(falses.ToArray()));
            if (trues.Count == 1)
                return trues.First();
            // If no logic defined and multiple filters, assume 'and'
            return descriptor.Bool(f => f.Must(trues.ToArray()).MustNot(falses.ToArray()));
        }

        public static NumericRangeQueryDescriptor<T> Filter<T>(this NumericRangeQueryDescriptor<T> descriptor, Filter one, Filter two) where T : class
        {
            Double fOne;
            if (!Double.TryParse(one.Value, out fOne))
                throw new ArgumentException("Filter not a parsable number");
            Double fTwo;
            if (!Double.TryParse(two.Value, out fTwo))
                throw new ArgumentException("Filter not a parsable number");

            switch (one.Operator)
            {
                case "lt":
                    descriptor = descriptor.LessThan(fOne);
                    break;

                case "lte":
                    descriptor = descriptor.LessThanOrEquals(fOne);
                    break;

                case "gt":
                    descriptor = descriptor.GreaterThan(fOne);
                    break;

                case "gte":
                    descriptor = descriptor.GreaterThanOrEquals(fOne);
                    break;
            }

            switch (two.Operator)
            {
                case "lt":
                    descriptor = descriptor.LessThan(fTwo);
                    break;

                case "lte":
                    descriptor = descriptor.LessThanOrEquals(fTwo);
                    break;

                case "gt":
                    descriptor = descriptor.GreaterThan(fTwo);
                    break;

                case "gte":
                    descriptor = descriptor.GreaterThanOrEquals(fTwo);
                    break;
            }
            return descriptor;
        }
    }
}
