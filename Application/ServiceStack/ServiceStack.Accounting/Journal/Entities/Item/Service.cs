using Demo.Domain.Accounting.Journal.Entities.Item.Commands;
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

namespace Demo.Application.ServiceStack.Accounting.Journal.Entities.Item
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

        public Object Any(Services.Get request)
        {
            return base.Request.ToOptimizedCachedAndSubscribedResult(request, base.Cache, _manager, _processor);
        }

        public Object Any(Services.ChangeEffective request)
        {
            var command = request.ConvertTo<ChangeEffective>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.ChangeReference request)
        {
            var command = request.ConvertTo<ChangeReference>();
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

        public Object Any(Services.Reconcile request)
        {
            var command = request.ConvertTo<Reconcile>();
            return _bus.Send(command).IsCommand<Command>();
        }
    }
}