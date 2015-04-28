using Aggregates;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.PaymentOrder
{
    public class Handler :
        IHandleMessages<Commands.ChangeEffective>,
        IHandleMessages<Commands.ChangeMethod>,
        IHandleMessages<Commands.ChangeReference>,
        IHandleMessages<Commands.Confirm>,
        IHandleMessages<Commands.Discard>,
        IHandleMessages<Commands.Dispurse>,
        IHandleMessages<Commands.Start>
    {
        private readonly IUnitOfWork _uow;
        private readonly IBus _bus;

        public Handler(IUnitOfWork uow, IBus bus)
        {
            _uow = uow;
            _bus = bus;
        }

        public void Handle(Commands.ChangeEffective command)
        {
            var paymentorder = _uow.R<PaymentOrder>().Get(command.PaymentOrderId);
            paymentorder.ChangeEffective(command.Effective);
        }

        public void Handle(Commands.ChangeMethod command)
        {
            var paymentorder = _uow.R<PaymentOrder>().Get(command.PaymentOrderId);
            paymentorder.ChangeMethod(command.MethodId);
        }

        public void Handle(Commands.ChangeReference command)
        {
            var paymentorder = _uow.R<PaymentOrder>().Get(command.PaymentOrderId);
            paymentorder.ChangeReference(command.Reference);
        }

        public void Handle(Commands.Confirm command)
        {
            var paymentorder = _uow.R<PaymentOrder>().Get(command.PaymentOrderId);
            paymentorder.Confirm();
        }

        public void Handle(Commands.Discard command)
        {
            var paymentorder = _uow.R<PaymentOrder>().Get(command.PaymentOrderId);
            paymentorder.Discard();
        }

        public void Handle(Commands.Dispurse command)
        {
            var paymentorder = _uow.R<PaymentOrder>().Get(command.PaymentOrderId);
            paymentorder.Dispurse();
        }

        public void Handle(Commands.Start command)
        {
            var paymentorder = _uow.R<PaymentOrder>().Get(command.PaymentOrderId);
            paymentorder.Start(command.Identity);
        }
    }
}