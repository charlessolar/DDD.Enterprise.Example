using Demo.Domain.Accounting.Currency.Commands;
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

namespace Demo.Application.ServiceStack.Accounting.Currency
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

        public Object Any(Services.GetRate request)
        {
            return base.Request.ToOptimizedCachedAndSubscribedResult(request, base.Cache, _manager, _processor);
        }

        public Object Any(Services.Activate request)
        {
            var command = request.ConvertTo<Activate>();

            command.UserId = this.Profile.UserId;
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.AddRate request)
        {
            var command = request.ConvertTo<AddRate>();
            return _bus.Send(command).IsCommand<Command>();
        }
        public Object Any(Services.ChangeRateEffective request)
        {
            var command = request.ConvertTo<ChangeRateEffective>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.ChangeFormat request)
        {
            var command = request.ConvertTo<ChangeFormat>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.ChangeFraction request)
        {
            var command = request.ConvertTo<ChangeFraction>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.ChangeAccuracy request)
        {
            var command = request.ConvertTo<ChangeAccuracy>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.ChangeName request)
        {
            var command = request.ConvertTo<ChangeName>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.ChangeRoundingFactor request)
        {
            var command = request.ConvertTo<ChangeRoundingFactor>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.ChangeSymbol request)
        {
            var command = request.ConvertTo<ChangeSymbol>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.Create request)
        {
            var command = request.ConvertTo<Create>();
            command.UserId = Profile.UserId;
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.Deactivate request)
        {
            var command = request.ConvertTo<Deactivate>();

            command.UserId = this.Profile.UserId;
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.SymbolBefore request)
        {
            var command = request.ConvertTo<SymbolBefore>();
            return _bus.Send(command).IsCommand<Command>();
        }
    }
}