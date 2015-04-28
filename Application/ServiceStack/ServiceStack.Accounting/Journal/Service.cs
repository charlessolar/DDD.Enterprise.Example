using Demo.Domain.Accounting.Journal.Commands;
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

namespace Demo.Application.ServiceStack.Accounting.Journal
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

        public Object Any(Services.ChangeCheckDate request)
        {
            var command = request.ConvertTo<ChangeCheckDate>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.ChangeName request)
        {
            var command = request.ConvertTo<ChangeName>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.ChangeResponsible request)
        {
            var command = request.ConvertTo<ChangeResponsible>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.ChangeSkipDraft request)
        {
            var command = request.ConvertTo<ChangeSkipDraft>();
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

        public Object Any(Services.SetCreditAccount request)
        {
            var command = request.ConvertTo<SetCreditAccount>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.SetDebitAccount request)
        {
            var command = request.ConvertTo<SetDebitAccount>();
            return _bus.Send(command).IsCommand<Command>();
        }
    }
}