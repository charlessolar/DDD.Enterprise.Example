using NServiceBus;
using System;

namespace Demo.Library.Queries
{
    public class IdResult : IMessage
    {
        public Guid Id { get; set; }
    }
}