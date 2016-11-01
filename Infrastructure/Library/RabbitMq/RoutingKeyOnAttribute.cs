using System;

namespace Demo.Library.RabbitMq
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class RoutingKeyOnAttribute : Attribute
    {
        public RoutingKeyOnAttribute(string property)
        {
            this.Property = property;
        }

        public readonly string Property;
    }
}
