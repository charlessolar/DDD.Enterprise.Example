using Aggregates;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.PaymentOrder.Entities.Invoice
{
    public class Handler :
        IHandleMessages<Commands.Add>,
        IHandleMessages<Commands.ChangeAmount>,
        IHandleMessages<Commands.ChangeDiscount>,
        IHandleMessages<Commands.ChangeReference>,
        IHandleMessages<Commands.Remove>
    {
        private readonly IUnitOfWork _uow;
        private readonly IBus _bus;

        public Handler(IUnitOfWork uow, IBus bus)
        {
            _uow = uow;
            _bus = bus;
        }

        public void Handle(Commands.Add command)
        {
            var paymentorder = _uow.R<PaymentOrder>().Get(command.PaymentOrderId);
            var invoice = paymentorder.E<Invoice>().New(command.InvoiceId);
            invoice.Add();
        }

        public void Handle(Commands.ChangeAmount command)
        {
            var paymentorder = _uow.R<PaymentOrder>().Get(command.PaymentOrderId);
            var invoice = paymentorder.E<Invoice>().Get(command.InvoiceId);
            invoice.ChangeAmount(command.Amount);
        }

        public void Handle(Commands.ChangeDiscount command)
        {
            var paymentorder = _uow.R<PaymentOrder>().Get(command.PaymentOrderId);
            var invoice = paymentorder.E<Invoice>().Get(command.InvoiceId);
            invoice.ChangeDiscount(command.Discount);
        }

        public void Handle(Commands.ChangeReference command)
        {
            var paymentorder = _uow.R<PaymentOrder>().Get(command.PaymentOrderId);
            var invoice = paymentorder.E<Invoice>().Get(command.InvoiceId);
            invoice.ChangeReference(command.Reference);
        }

        public void Handle(Commands.Remove command)
        {
            var paymentorder = _uow.R<PaymentOrder>().Get(command.PaymentOrderId);
            var invoice = paymentorder.E<Invoice>().Get(command.InvoiceId);
            invoice.Remove();
        }
    }
}