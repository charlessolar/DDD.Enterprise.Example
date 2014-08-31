using Demo.Library.Extensions;
using Demo.Library.Queries;
using Demo.Library.Cache;
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
using System.Net;
using Demo.Presentation.Inventory.Models.Items.Responses;
using Demo.Presentation.Inventory.Models.Items.Services;
using Demo.Library.Responses;

namespace Demo.Presentation.Inventory.Items
{
    public class Items : Service
    {

        private IBus _bus;

        public Items(IBus bus)
        {
            _bus = bus;
        }


        public Task<Item> Any(GetItem request)
        {


            return _bus.Send("application", new Application.Inventory.Items.Queries.GetItem
            {
                Id = request.Id,
            }).Register(x =>
            {
                var result = (x.Messages.First() as Result).Records.FirstOrDefault();

                if (result == null)
                    throw new HttpError(HttpStatusCode.NotFound, "Get request failed");

                var item = result.ConvertTo<Item>();

                item.AddSession(base.Cache, Request.GetPermanentSessionId());

                return item;
            });
        }

        public Task<Find> Any(FindItems request)
        {
            return _bus.Send("application", new Application.Inventory.Items.Queries.FindItems
            {
                Page = request.Page,
                PageSize = request.PageSize,
                Number = request.Number,
                Description = request.Description,
            }).Register(x =>
            {
                return new Find
                {
                    Results = (x.Messages.First() as Result).Records.Select(r => r.ConvertTo<Item>())
                };
            });
        }

        public Task<IdResponse> Post(CreateItem request)
        {
            var command = request.ConvertTo<Domain.Inventory.Items.Commands.Create>();

            return _bus.Send("domain", command)
                .Register(x =>
                    {
                        // Perhaps we can send a reply to our command with the saved item id
                        return new IdResponse { Id = command.ItemId };
                    });
        }

        public Task<IdResponse> Post(ChangeDescription request)
        {
            var command = request.ConvertTo<Domain.Inventory.Items.Commands.ChangeDescription>();

            return _bus.Send("domain", command)
                .Register(x =>
                {
                    // Perhaps we can send a reply to our command with the saved item id
                    return new IdResponse { Id = command.ItemId };
                });
        }
    }
}