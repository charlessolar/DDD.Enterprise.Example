using Demo.Domain.Accounting.Tax.Commands;
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

namespace Demo.Application.ServiceStack.Accounting.Tax
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


        public Object Any(Services.Activate request)
        {
            var command = request.ConvertTo<Activate>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.AddRegion request)
        {
            var command = request.ConvertTo<AddRegion>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.AddStore request)
        {
            var command = request.ConvertTo<AddStore>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.ChangeAccount request)
        {
            var command = request.ConvertTo<ChangeAccount>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.ChangeDescription request)
        {
            var command = request.ConvertTo<ChangeDescription>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.ChangeName request)
        {
            var command = request.ConvertTo<ChangeName>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.ChangeRate request)
        {
            var command = request.ConvertTo<ChangeRate>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.Create request)
        {
            var command = request.ConvertTo<Create>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.Deactivate request)
        {
            var command = request.ConvertTo<Deactivate>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.Destroy request)
        {
            var command = request.ConvertTo<Destroy>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.RemoveRegion request)
        {
            var command = request.ConvertTo<RemoveRegion>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.RemoveStore request)
        {
            var command = request.ConvertTo<RemoveStore>();
            return _bus.Send(command).IsCommand<Command>();
        }
    }
}