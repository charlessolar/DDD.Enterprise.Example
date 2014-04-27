using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Inventory.SerialNumbers.Messages
{
    public class SerialNumberRetreived : IMessage
    {
        public SerialNumber SerialNumber { get; set; }
    }
}