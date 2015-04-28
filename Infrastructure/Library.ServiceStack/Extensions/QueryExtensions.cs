using Demo.Library.Queries;
using Demo.Library.Queries.Grid;
using Demo.Library.Responses;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Linq;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Library.Extensions
{
    public static class QueryExtensions
    {
        public static Paged<T> Paging<T>(this IRavenQueryable<T> source, PagedQuery<T> request) where T : IResponse
        {
            RavenQueryStatistics stats;
            var result = source.Statistics(out stats)
                            .Skip(request.Skip ?? 0)
                            .Take(request.Take ?? Int32.MaxValue)
                            .ToList();

            return new Paged<T> { Records = result.ToList(), Total = stats.TotalResults };
        }

        public static Paged<T> Paging<T>(this IQueryable<T> source, PagedQuery<T> request) where T : IResponse
        {
            var count = source.Count();
            var result = source
                            .Skip(request.Skip ?? 0)
                            .Take(request.Take ?? Int32.MaxValue)
                            .ToList();

            return new Paged<T> { Records = result.ToList(), Total = count };
        }

        public static Paged<T> Paging<T>(this T source, PagedQuery<T> request) where T : IResponse
        {
            var count = 1;
            var result = new List<T> { source };

            return new Paged<T> { Records = result, Total = count };
        }

        public static Nest.SearchDescriptor<T> Paging<T>(this Nest.SearchDescriptor<T> descriptor, PagedQuery<T> request) where T : class, IResponse
        {
            descriptor = descriptor.Skip(request.Skip ?? 0).Take(request.Take ?? Int32.MaxValue);
            if (request.Filter != null)
                descriptor = descriptor.Filter(f => f.Grid(request.Filter));
            if (request.Sort != null && request.Sort.Any())
                descriptor = descriptor.Sort(request.Sort);

            return descriptor;
        }

        public static Nest.SearchDescriptor<T> Sort<T>(this Nest.SearchDescriptor<T> descriptor, IEnumerable<Sort> sorts) where T : class, IResponse
        {
            foreach (var sort in sorts)
            {
                if (sort.Dir == "desc")
                    descriptor = descriptor.SortDescending(sort.Field);
                if (sort.Dir == "asc")
                    descriptor = descriptor.SortAscending(sort.Field);
            }
            return descriptor;
        }

        public static Nest.FilterContainer Grid<T>(this Nest.FilterDescriptor<T> descriptor, Filter filter) where T : class, IResponse
        {
            var filters = new List<Nest.FilterContainer>();

            if (filter.Value != null)
                filter.Value = filter.Value.ToLowerInvariant();

            switch (filter.Operator)
            {
                case "eq":
                    filters.Add(descriptor.Term(filter.Field, filter.Value));
                    break;

                case "neq":
                    filters.Add(descriptor.Not(f => f.Term(filter.Field, filter.Value)));
                    break;

                case "lt":
                    filters.Add(descriptor.Range(f => f.OnField(filter.Field).Lower(filter.Value.ToString())));
                    break;

                case "lte":
                    filters.Add(descriptor.Range(f => f.OnField(filter.Field).LowerOrEquals(filter.Value.ToString())));
                    break;

                case "gt":
                    filters.Add(descriptor.Range(f => f.OnField(filter.Field).Greater(filter.Value.ToString())));
                    break;

                case "gte":
                    filters.Add(descriptor.Range(f => f.OnField(filter.Field).GreaterOrEquals(filter.Value.ToString())));
                    break;

                case "startswith":
                    filters.Add(descriptor.Prefix(filter.Field, filter.Value.ToString()));
                    break;

                case "endswith":
                    filters.Add(descriptor.Query(q => q.MatchPhrasePrefix(mpp => mpp.Analyzer("suffix").OnField(filter.Field).Query(filter.Value.ToString()))));
                    break;

                case "contains":
                    filters.Add(descriptor.Term(filter.Field, filter.Value.ToString()));
                    break;

                case "doesnotcontain":
                    filters.Add(descriptor.Not(f => f.Term(filter.Field, filter.Value.ToString())));
                    break;
            }

            // Special condition for range filter
            if (filter.Logic == "and" && filter.Filters != null && filter.Filters.Count() == 2 && filter.Filters.All(f => new[] { "lt", "lte", "gt", "gte" }.Contains(f.Operator)) && !filter.Filters.GroupBy(f => f.Field).Skip(1).Any())
            {
                var field = filter.Filters.First().Field;
                // Range query
                filters.Add(descriptor.Range(f => f.OnField(field).Filter(filter.Filters.First(), filter.Filters.Last())));
            }
            else if (filter.Filters != null && filter.Filters.Any())
            {
                foreach (var child in filter.Filters)
                    filters.Add(descriptor.Grid(child));
            }

            if (!filter.Logic.IsNullOrEmpty() && filter.Logic.ToLower() == "or")
                return descriptor.Or(filters.ToArray());
            if (filters.Count == 1)
                return filters.First();
            // If no logic defined and multiple filters, assume 'and'
            return descriptor.And(filters.ToArray());
        }

        public static Nest.RangeFilterDescriptor<T> Filter<T>(this Nest.RangeFilterDescriptor<T> descriptor, Filter one, Filter two) where T : class, IResponse
        {
            switch (one.Operator)
            {
                case "lt":
                    descriptor = descriptor.Lower(one.Value.ToString());
                    break;

                case "lte":
                    descriptor = descriptor.LowerOrEquals(one.Value.ToString());
                    break;

                case "gt":
                    descriptor = descriptor.Greater(one.Value.ToString());
                    break;

                case "gte":
                    descriptor = descriptor.GreaterOrEquals(one.Value.ToString());
                    break;
            }

            switch (two.Operator)
            {
                case "lt":
                    descriptor = descriptor.Lower(two.Value.ToString());
                    break;

                case "lte":
                    descriptor = descriptor.LowerOrEquals(two.Value.ToString());
                    break;

                case "gt":
                    descriptor = descriptor.Greater(two.Value.ToString());
                    break;

                case "gte":
                    descriptor = descriptor.GreaterOrEquals(two.Value.ToString());
                    break;
            }
            return descriptor;
        }
        public static T WaitAndLoad<T>(this IDocumentStore store, object id)
            where T : class
        {
            // Keep trying to query the store until the data is found
            var count = 0;
            while (count < 10)
            {
                using (var session = store.OpenSession())
                {
                    var existing = session.Load<T>(id.ToString());

                    if (existing != null)
                        return existing;
                }
                count++;
                Thread.Sleep(50);
            }

            return null;
        }
    }
}