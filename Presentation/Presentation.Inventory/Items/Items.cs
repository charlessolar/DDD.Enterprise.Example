using Demo.Application.Inventory.Items;
using Demo.Library.Extensions;
using Demo.Library.Queries;
using Demo.Presentation.Inventory.Items.Models;
using NServiceBus;
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

namespace Demo.Presentation.Inventory.Items
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
                    Id = request.Id,
                    Fields = request.Fields.IsEmpty() ? typeof(Item).GetPropertyNames().ToArray() : request.Fields
                }).Register(x =>
                {
                    return (x.Messages.First() as Result).Records;
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
                    Page = request.Page,
                    PageSize = request.PageSize,
                    Number = request.Number,
                    Description = request.Description,
                    Fields = request.Fields.IsEmpty() ? typeof(Item).GetPropertyNames().ToArray() : request.Fields
                }).Register(x =>
                {
                    return (x.Messages.First() as Result).Records;
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