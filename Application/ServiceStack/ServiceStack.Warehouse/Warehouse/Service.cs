using Demo.Domain.Warehouse.Warehouse.Commands;
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

namespace Demo.Application.ServiceStack.Warehouse.Warehouse
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

        public Object Any(Services.UpdateAddress request)
        {
            var command = request.ConvertTo<UpdateAddress>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.UpdateDescription request)
        {
            var command = request.ConvertTo<UpdateDescription>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.UpdateManager request)
        {
            var command = request.ConvertTo<UpdateManager>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.UpdateName request)
        {
            var command = request.ConvertTo<UpdateName>();
            return _bus.Send(command).IsCommand<Command>();
        }
    }
}