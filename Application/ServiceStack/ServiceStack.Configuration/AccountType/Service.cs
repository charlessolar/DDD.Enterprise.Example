using Demo.Domain.Configuration.AccountType;
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

namespace Demo.Application.ServiceStack.Configuration.AccountType
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

        public Object Any(Services.Get request)
        {
            return base.Request.ToOptimizedCachedAndSubscribedResult(request, base.Cache, _manager, _processor);
        }

        public Object Any(Services.Select request)
        {
            return base.Request.ToOptimizedCachedAndSubscribedResult(request, base.Cache, _manager, _processor);
        }

        public Object Post(Services.Create request)
        {
            var command = request.ConvertTo<Domain.Configuration.AccountType.Commands.Create>();
            command.DeferralMethod = DEFERRAL_METHOD.FromValue(request.DeferralMethod);

            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Post(Services.Destroy request)
        {
            var command = request.ConvertTo<Domain.Configuration.AccountType.Commands.Destroy>();

            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Post(Services.ChangeName request)
        {
            var command = request.ConvertTo<Domain.Configuration.AccountType.Commands.ChangeName>();

            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Post(Services.ChangeDeferral request)
        {
            var command = request.ConvertTo<Domain.Configuration.AccountType.Commands.ChangeDeferral>();

            return _bus.Send(command).IsCommand<Command>();
        }
    }
}