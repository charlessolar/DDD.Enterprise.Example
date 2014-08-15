using NServiceBus.Serialization;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace Demo.Library.NServiceBus
{


    public class ServiceStackSerializer : IMessageSerializer
    {
        public string ContentType
        {
            get { return ContentTypes.Json;}
        }

        public object[] Deserialize(System.IO.Stream stream, IList<Type> messageTypes = null)
        {
            
            if (messageTypes != null && messageTypes.Any())
            {
                return new[] {TypeSerializer.DeserializeFromStream(messageTypes.First(), stream)};
            }

            return new[] { TypeSerializer.DeserializeFromStream(typeof(Dictionary<string, object>), stream) };
        }

        public void Serialize(object[] messages, System.IO.Stream stream)
        {
            var message = messages.First();
            TypeSerializer.SerializeToStream(message, stream);
        }
    }
}
