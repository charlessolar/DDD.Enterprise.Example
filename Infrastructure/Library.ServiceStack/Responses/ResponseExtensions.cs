using Demo.Library.Cache;
using Demo.Library.Queries;
using Demo.Library.Responses;
using NServiceBus;
using ServiceStack;
using ServiceStack.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        public static Full<T> ToResponse<T>(this Wrapper<T> wrapper) where T : class, IHasGuidId
        {
            var urn = UrnId.Create<T>(wrapper.Payload.Id);
            return new Full<T>
            {
                Status = "success",
                Urn = urn,
                Version = wrapper.Version,
                Sessions = wrapper.Sessions,
                Created = wrapper.Created,
                Updated = wrapper.Updated,
                Payload = wrapper.Payload,
            };
        }

        public static Diff<T> ToDiff<T>( this Wrapper<T> wrapper, Object payload ) where T : class, IHasGuidId
        {
            var urn = UrnId.Create<T>(wrapper.Payload.Id);
            return new Diff<T>
            {
                Status = "success",
                Urn = urn,
                Version = wrapper.Version,
                Updated = wrapper.Updated,
                Payload = payload,
            };
        }
    }
}
