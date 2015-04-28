using Demo.Domain.Accounting.PaymentOrder.Entities.Invoice.Commands;
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

namespace Demo.Application.ServiceStack.Accounting.PaymentOrder.Entities.Invoice
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
                return session.Query<Responses.Index>().Where(x => x.PaymentOrderId == request.PaymentOrderId).ToList();
            }
        }

        public Object Any(Services.Add request)
        {
            var command = request.ConvertTo<Add>();
            return _bus.Send( command).IsCommand<Command>();
        }

        public Object Any(Services.ChangeAmount request)
        {
            var command = request.ConvertTo<ChangeAmount>();
            return _bus.Send( command).IsCommand<Command>();
        }

        public Object Any(Services.ChangeDiscount request)
        {
            var command = request.ConvertTo<ChangeDiscount>();
            return _bus.Send( command).IsCommand<Command>();
        }

        public Object Any(Services.ChangeReference request)
        {
            var command = request.ConvertTo<ChangeReference>();
            return _bus.Send( command).IsCommand<Command>();
        }

        public Object Any(Services.Remove request)
        {
            var command = request.ConvertTo<Remove>();
            return _bus.Send( command).IsCommand<Command>();
        }
    }
}