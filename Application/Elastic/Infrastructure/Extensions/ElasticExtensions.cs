using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Extensions
{
    public static class ElasticExtensions
    {
        public static MultiGetOperationDescriptor<T> Id<T>(this MultiGetOperationDescriptor<T> get, Guid? id) where T : class
        {
            return get.Id(id.ToString());
        }

        public static MultiGetHit<T> Get<T>(this IMultiGetResponse response, Guid? id) where T : class
        {
            return response.Get<T>(id.ToString());
        }

        public static IGetResponse<T> Get<T>(this IElasticClient client, Guid id, string index = null, string type = null) where T : class
        {
            return client.Get<T>(id.ToString());
        }
        
        public static IDeleteResponse Delete<T>(this IElasticClient client, Guid? id, Func<DeleteDescriptor<T>, DeleteDescriptor<T>> selector = null) where T : class
        {
            return client.Delete(id.ToString(), selector);
        }

        public static QueryStringQueryDescriptor<T> WildQuery<T>(this QueryStringQueryDescriptor<T> descriptor, string query) where T : class
        {
            return descriptor.Query(query + '*');
        }
    }
}