using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System.Collections.Generic;
using System.Linq;

namespace Seed.Internal
{
    public static class RabbitMqMgmt
    {
        private class Response
        {
            public int Messages { get; set; }
            public string State { get; set; }
            public string Name { get; set; }
            public string Vhost { get; set; }
        }

        public static IEnumerable<string> GetAllQueues(string url, string username ="guest", string password ="guest", string virtualHost = "/")
        {
            var client = new RestClient(url);
            client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);

            try {
                var queues = JsonConvert.DeserializeObject<IEnumerable<Response>>(response.Content);
                return queues.Where(x => x.Vhost == virtualHost).Select(x => x.Name);
            }
            catch (Newtonsoft.Json.JsonSerializationException)
            {
                return new List<string>();
            }
        }
        public static IEnumerable<string> GetAllExchanges(string url, string username = "guest", string password = "guest", string virtualHost = "/")
        {
            var client = new RestClient(url) {Authenticator = new HttpBasicAuthenticator(username, password)};

            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);

            try
            {
                var queues = JsonConvert.DeserializeObject<IEnumerable<Response>>(response.Content);
                return queues.Where(x => x.Vhost == virtualHost).Select(x => x.Name);
            }
            catch (Newtonsoft.Json.JsonSerializationException)
            {
                return new List<string>();
            }
        }
    }
}
