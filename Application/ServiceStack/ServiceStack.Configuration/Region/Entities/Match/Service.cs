using Demo.Library.Authentication;
using Demo.Library.Extensions;
using Demo.Library.Responses;
using Demo.Library.Services;
using NServiceBus;
using Raven.Client;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Configuration.Region.Entities.Match
{
    [RequireJWT]
    public class Service : DemoService
    {
        private readonly IBus _bus;
        private readonly IDocumentStore _store;

        public Service(IBus bus, IDocumentStore store)
        {
            _bus = bus;
            _store = store;
        }

        public Object Any(Services.Index request)
        {
            using (var session = _store.OpenSession())
            {
                return session.Query<Responses.Index>().ToList();
            }
        }

        public Object Post(Services.Create request)
        {
            var command = request.ConvertTo<Domain.Configuration.Region.Entities.Match.Commands.Create>();

            return _bus.Send( command).IsCommand<Command>();
        }

        public Object Post(Services.Destroy request)
        {
            var command = request.ConvertTo<Domain.Configuration.Region.Entities.Match.Commands.Destroy>();

            return _bus.Send( command).IsCommand<Command>();
        }

        public Object Post(Services.ChangeOperation request)
        {
            var command = request.ConvertTo<Domain.Configuration.Region.Entities.Match.Commands.ChangeOperation>();

            return _bus.Send( command).IsCommand<Command>();
        }

        public Object Post(Services.ChangeType request)
        {
            var command = request.ConvertTo<Domain.Configuration.Region.Entities.Match.Commands.ChangeType>();

            return _bus.Send( command).IsCommand<Command>();
        }

        public Object Post(Services.ChangeValue request)
        {
            var command = request.ConvertTo<Domain.Configuration.Region.Entities.Match.Commands.ChangeValue>();

            return _bus.Send( command).IsCommand<Command>();
        }
    }
}