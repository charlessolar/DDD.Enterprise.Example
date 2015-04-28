using Demo.Domain.Accounting.PaymentOrder.Commands;
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

namespace Demo.Application.ServiceStack.Accounting.PaymentOrder
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

        public Object Any(Services.Index request)
        {
            return base.Request.ToOptimizedCachedAndSubscribedResult(request, base.Cache, _manager, _processor);
        }

        public Object Any(Services.ChangeEffective request)
        {
            var command = request.ConvertTo<ChangeEffective>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.ChangeMethod request)
        {
            var command = request.ConvertTo<ChangeMethod>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.ChangeReference request)
        {
            var command = request.ConvertTo<ChangeReference>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.Confirm request)
        {
            var command = request.ConvertTo<Confirm>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.Dispurse request)
        {
            var command = request.ConvertTo<Dispurse>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.Start request)
        {
            var command = request.ConvertTo<Start>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.Discard request)
        {
            var command = request.ConvertTo<Discard>();
            return _bus.Send(command).IsCommand<Command>();
        }
    }
}