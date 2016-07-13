using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NLog;
using NServiceBus.MessageInterfaces;
using NServiceBus.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using NewtonSerializer = Newtonsoft.Json.JsonSerializer;

namespace NServiceBus.Newtonsoft.Json
{
    /// <summary>
    /// Newtonsoft JSON message serializer.
    /// </summary>
    public class JsonMessageSerializer : IMessageSerializer
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private IMessageMapper messageMapper;
        private MessageContractResolver messageContractResolver;
        private Func<Stream, JsonReader> readerCreator;
        private JsonSerializerSettings settings;
        private Func<Stream, JsonWriter> writerCreator;

        /// <summary>
        /// Initializes a new instance of <see cref="JsonMessageSerializer"/>.
        /// </summary>
        public JsonMessageSerializer(IMessageMapper messageMapper)
        {
            Guard.AgainstNull(messageMapper, "messageMapper");
            this.messageMapper = messageMapper;
            messageContractResolver = new MessageContractResolver(messageMapper);
            settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                Converters =
                           {
                               new IsoDateTimeConverter
                               {
                                   DateTimeStyles = DateTimeStyles.RoundtripKind
                               }
                           }
            };
            writerCreator = stream =>
            {
                var streamWriter = new StreamWriter(stream, Encoding.UTF8);
                return new JsonTextWriter(streamWriter)
                {
                    Formatting = Formatting.None
                };
            };
            readerCreator = stream =>
            {
                var streamReader = new StreamReader(stream, Encoding.UTF8);
                return new JsonTextReader(streamReader);
            };
        }

        public void Serialize(object message, Stream stream)
        {
            Guard.AgainstNull(stream, "stream");
            Guard.AgainstNull(message, "message");

            var jsonSerializer = NewtonSerializer.Create(settings);
            jsonSerializer.Binder = new MessageSerializationBinder(messageMapper);
            var jsonWriter = writerCreator(stream);
            jsonSerializer.Serialize(jsonWriter, message);
            
            jsonWriter.Flush();

        }

        public object[] Deserialize(Stream stream, IList<Type> messageTypes)
        {
            Guard.AgainstNull(stream, "stream");
            Guard.AgainstNull(messageTypes, "messageTypes");

            var jsonSerializer = NewtonSerializer.Create(settings);
            jsonSerializer.ContractResolver = messageContractResolver;
            jsonSerializer.Binder = new MessageSerializationBinder(messageMapper, messageTypes);

            if (IsArrayStream(stream))
            {
                var arrayReader = readerCreator(stream);
                return jsonSerializer.Deserialize<object[]>(arrayReader);
            }

            if (messageTypes.Any())
            {
                return DeserializeMultipleMesageTypes(stream, messageTypes, jsonSerializer).ToArray();
            }

            var simpleReader = readerCreator(stream);
            return new[]
                   {
                       jsonSerializer.Deserialize<object>(simpleReader)
                   };
        }

        private IEnumerable<object> DeserializeMultipleMesageTypes(Stream stream, IList<Type> messageTypes, NewtonSerializer jsonSerializer)
        {
            foreach (var messageType in FindRootTypes(messageTypes))
            {
                stream.Seek(0, SeekOrigin.Begin);

                var reader = readerCreator(stream);
                yield return jsonSerializer.Deserialize(reader, messageType);
            }
        }

        private bool IsArrayStream(Stream stream)
        {
            var reader = readerCreator(stream);
            reader.Read();
            stream.Seek(0, SeekOrigin.Begin);
            return reader.TokenType == JsonToken.StartArray;
        }

        private static IEnumerable<Type> FindRootTypes(IEnumerable<Type> messageTypesToDeserialize)
        {
            Type currentRoot = null;
            foreach (var type in messageTypesToDeserialize)
            {
                if (currentRoot == null)
                {
                    currentRoot = type;
                    yield return currentRoot;
                    continue;
                }
                if (!type.IsAssignableFrom(currentRoot))
                {
                    currentRoot = type;
                    yield return currentRoot;
                }
            }
        }

        public string ContentType
        {
            get { return ContentTypes.Json; }
        }
        
    }
}