using Demo.Domain.HumanResources.Employee.Commands;
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

namespace Demo.Application.ServiceStack.HumanResources.Employee
{
    [RequireJWT]
    internal class Service : DemoService
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

        public Object Any(Services.Hire request)
        {
            var command = request.ConvertTo<Hire>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.LinkUser request)
        {
            var command = request.ConvertTo<LinkUser>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.Terminate request)
        {
            var command = request.ConvertTo<Terminate>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.UnlinkUser request)
        {
            var command = request.ConvertTo<UnlinkUser>();
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

        public Object Any(Services.UpdateDirectPhone request)
        {
            var command = request.ConvertTo<UpdateDirectPhone>();
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

        public Object Any(Services.UpdateFullName request)
        {
            var command = request.ConvertTo<UpdateFullName>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.UpdateGender request)
        {
            var command = request.ConvertTo<UpdateGender>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.UpdateMaritalStatus request)
        {
            var command = request.ConvertTo<UpdateMaritalStatus>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.UpdateMobile request)
        {
            var command = request.ConvertTo<UpdateMobile>();
            return _bus.Send(command).IsCommand<Command>();
        }

        public Object Any(Services.UpdateNationalId request)
        {
            var command = request.ConvertTo<UpdateNationalId>();
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