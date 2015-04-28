using Demo.Domain.Accounting.Account;
using Demo.Domain.Accounting.Account.Commands;
using Demo.Library.Authentication;
using Demo.Library.Extensions;

using Demo.Library.Queries.Processor;
using Demo.Library.Responses;
using Demo.Library.Services;
using Demo.Library.SSE;
using NServiceBus;
using Raven.Client;
using Raven.Client.Extensions;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Accounting.Account
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

        public Object Any(Services.Select request)
        {
            return base.Request.ToOptimizedCachedAndSubscribedResult(request, base.Cache, _manager, _processor);
        }

        public Object Any(Services.ChangeDescription request)
        {
            var command = request.ConvertTo<ChangeDescription>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.ChangeType request)
        {
            var command = request.ConvertTo<Demo.Domain.Accounting.Account.Commands.ChangeType>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.ChangeName request)
        {
            var command = request.ConvertTo<ChangeName>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.ChangeParent request)
        {
            var command = request.ConvertTo<ChangeParent>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.Create request)
        {
            var command = request.ConvertTo<Create>();
            command.Operation = OPERATION.FromValue(request.Operation);

            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.Freeze request)
        {
            var command = request.ConvertTo<Freeze>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.Unfreeze request)
        {
            var command = request.ConvertTo<Unfreeze>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.Destroy request)
        {
            var command = request.ConvertTo<Destroy>();
            return _bus.Send(command).IsCommand<Command>();
        }
    }
}