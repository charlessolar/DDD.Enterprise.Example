using Demo.Domain.Relations.Store.Commands;
using Demo.Library.Authentication;
using Demo.Library.Extensions;
using Demo.Library.Queries.Processor;
using Demo.Library.Responses;
using Demo.Library.Services;
using Demo.Library.SSE;
using NServiceBus;
using Raven.Client;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Relations.Store
{
    [RequireJWT]
    public class Service : DemoService
    {
        private readonly IBus _bus;
        private readonly IQueryProcessor _processor;
        private readonly ISubscriptionManager _manager;

        public Service(IBus bus, IQueryProcessor processor, ISubscriptionManager manager)
        {
            _bus = bus;
            _processor = processor;
            _manager = manager;
        }

        public Object Any(Services.Index request)
        {
            return base.Request.ToOptimizedCachedAndSubscribedResult(request, base.Cache, _manager, _processor);
        }

        public Object Any(Services.Select request)
        {
            return base.Request.ToOptimizedCachedAndSubscribedResult(request, base.Cache, _manager, _processor);
        }

        public Object Any(Services.Get request)
        {
            return base.Request.ToOptimizedCachedAndSubscribedResult(request, base.Cache, _manager, _processor);
        }

        public Object Any(Services.AddWarehouse request)
        {
            var command = request.ConvertTo<AddWarehouse>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.Create request)
        {
            var command = request.ConvertTo<Create>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.Destroy request)
        {
            var command = request.ConvertTo<Destroy>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.RemoveWarehouse request)
        {
            var command = request.ConvertTo<RemoveWarehouse>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.UpdateAddress request)
        {
            var command = request.ConvertTo<UpdateAddress>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.UpdateCurrency request)
        {
            var command = request.ConvertTo<UpdateCurrency>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.UpdateDescription request)
        {
            var command = request.ConvertTo<UpdateDescription>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.UpdateEmail request)
        {
            var command = request.ConvertTo<UpdateEmail>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.UpdateFax request)
        {
            var command = request.ConvertTo<UpdateFax>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.UpdateName request)
        {
            var command = request.ConvertTo<UpdateName>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.UpdatePhone request)
        {
            var command = request.ConvertTo<UpdatePhone>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.UpdateWebsite request)
        {
            var command = request.ConvertTo<UpdateWebsite>();
            return _bus.Send(command).IsCommand<Command>();
        }
    }
}