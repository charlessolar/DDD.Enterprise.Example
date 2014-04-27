using Application.Inventory.SerialNumbers;
using Application.Inventory.SerialNumbers.Messages;
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

        public Object Any(GetSerialNumber request)
        {
            var cacheKey = UrnId.Create<GetSerialNumber>(request.Id);
            return base.Request.ToOptimizedResultUsingCache(base.Cache, cacheKey, () =>
            {
                return _bus.Send("application", new Application.Inventory.SerialNumbers.Queries.GetSerialNumber
                {
                    Id = request.Id
                }).Register(x =>
                {
                    return (x.Messages.First() as SerialNumbersRetreived).SerialNumbers;
                }).Result;
            });
        }

        public Object Any(FindSerialNumbers request)
        {
            var cacheKey = UrnId.Create<FindSerialNumbers>(String.Format("{0}.{1}.{2:N}.{3}.{4}", request.Serial, request.Effective, request.ItemId, request.Page, request.PageSize));
            return base.Request.ToOptimizedResultUsingCache(base.Cache, cacheKey, () =>
            {
                return _bus.Send("application", new Application.Inventory.SerialNumbers.Queries.FindSerialNumbers
                {
                    Page = request.Page,
                    PageSize = request.PageSize,
                    Serial = request.Serial,
                    Effective = request.Effective,
                    ItemId = request.ItemId
                }).Register(x =>
                {
                    return (x.Messages.First() as SerialNumbersRetreived).SerialNumbers;
                }).Result;
            });
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