using System;
using NServiceBus.Serialization;

namespace NServiceBus.Newtonsoft.Json
{
    /// <summary>
    /// Defines the capabilities of the JSON serializer
    /// </summary>
    public class NewtonsoftSerializer : SerializationDefinition
    {
        /// <summary>
        /// <see cref="SerializationDefinition.ProvidedByFeature"/>
        /// </summary>
        protected override Type ProvidedByFeature()
        {
            return typeof(NewtonsoftSerialization);
        }
    }

}
