using Demo.Library.Queries;
using NServiceBus;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Library.Extensions
{
    public static class ResponseExtensions
    {
        public static T GetQueryResponse<T>(this CompletionResult result) where T : class
        {
            var record = (result.Messages.First() as Result).Records.FirstOrDefault();
            if (record == null) return default(T);
            return record as T;
        }

        public static IEnumerable<T> GetQueryListResponse<T>(this CompletionResult result) where T : class
        {
            var records = (result.Messages.First() as Result).Records;
            if (result == null) return null;
            return records.Cast<T>();
        }
    }
}