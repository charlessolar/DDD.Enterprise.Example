using Application.Inventory.SerialNumbers;
using NServiceBus;
using Presentation.Inventory.SerialNumbers.Models;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.Inventory.SerialNumbers
{
    public class SerialNumbers : Service
    {
        public IBus _bus { get; set; }

        public SerialNumber Any(GetSerialNumber request)
        {
            _bus.Send("application", new Application.Inventory.SerialNumbers.Queries.GetSerialNumber
            {
                Id = request.Id
            });
            return null;
        }

        public List<SerialNumber> Any(FindSerialNumbers request)
        {
            _bus.Send("application", new Application.Inventory.SerialNumbers.Queries.FindSerialNumbers
            {
                Serial = request.Serial,
                Effective = request.Effective,
                ItemId = request.ItemId,
            });

            return new List<SerialNumber>();
        }

        public Guid Post(CreateSerialNumber request)
        {
            var command = request.ConvertTo<Domain.Inventory.SerialNumbers.Commands.Create>();
            command.ItemId = Guid.NewGuid();
            _bus.Send("domain", command);

            return command.ItemId;
        }
    }
}