using Raven.Client;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Q = Demo.Library.Queries;
using R = Demo.Library.Responses;

namespace Demo.Library.Extensions
{
    public static class ResponseExtensions
    {
        public static String DocumentId<T>(this T dto) where T : R.IResponse
        {
            var id = typeof(T).GetProperty("Id");

            if (id == null)
                throw new ArgumentException("Type {0} does not have an 'Id' property".Fmt(dto.GetType().FullName));

            return id.GetValue(dto).ToString();
        }

        public static R.Query<T> ToQueryResponse<T>(this T dto, Q.Query<T> query) where T : class, R.IResponse, new()
        {
            return new R.Query<T> { Payload = dto, Etag = Guid.Empty, SubscriptionId = query.SubscriptionId };
        }
        public static R.Query<R.Paged<T>> ToQueryResponse<T>(this R.Paged<T> dto, Q.PagedQuery<T> query) where T : R.IResponse, new()
        {
            return new R.Query<R.Paged<T>> { Payload = dto, Etag = Guid.Empty, SubscriptionId = query.SubscriptionId };
        }
    }
}