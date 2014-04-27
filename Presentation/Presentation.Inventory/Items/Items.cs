using Application.Inventory.Items;
using Application.Inventory.Items.Messages;
using NServiceBus;
using Presentation.Inventory.Items.Models;
using ServiceStack;
using ServiceStack.Caching;
using ServiceStack.Messaging;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.Inventory.Items
{
    public class Items : Service
    {
        public IBus _bus { get; set; }


        public Object Any(GetItem request)
        {
            var cacheKey = UrnId.Create<GetItem>(request.Id);
            return base.Request.ToOptimizedResultUsingCache(base.Cache, cacheKey, () =>
            {
                return _bus.Send("application", new Application.Inventory.Items.Queries.GetItem
                {
                    QueryId = cacheKey,
                    Id = request.Id
                }).Register(x =>
                {
                    return (x.Messages.First() as ItemsRetreived).Items;
                }).Result;
            });
        }

        public Object Any(FindItems request)
        {
            var cacheKey = UrnId.Create<FindItems>(String.Format("{0}.{1}.{2}.{3}", request.Number, request.Description, request.Page, request.PageSize));

            return base.Request.ToOptimizedResultUsingCache(base.Cache, cacheKey, () =>
            {
                return _bus.Send("application", new Application.Inventory.Items.Queries.FindItems
                {
                    QueryId = cacheKey,
                    Page = request.Page,
                    PageSize = request.PageSize,
                    Number = request.Number,
                    Description = request.Description,
                }).Register(x =>
                {
                    return (x.Messages.First() as ItemsRetreived).Items;
                }).Result;
            });
        }

        public Guid Post(CreateItem request)
        {
            var command = request.ConvertTo<Domain.Inventory.Items.Commands.Create>();
            command.ItemId = Guid.NewGuid();
            _bus.Send("domain", command);

            return command.ItemId;
        }
    }
}