using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Extensions
{
    public static class RestExtensions
    {
        public static Task ExecuteAsync(this RestClient client, RestRequest request)
        {
            var tcs = new TaskCompletionSource<bool>();
            var handle = client.ExecuteAsync(request, r =>
            {
                if (r.ErrorException == null)
                {
                    tcs.SetResult(true);
                }
                else
                {
                    tcs.SetException(r.ErrorException);
                }
            });
            return tcs.Task;
        }
    }
}
