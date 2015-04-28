using Aggregates;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.PaymentMethod.Entities.Detail
{
    public class Handler :
        IHandleMessages<Commands.Create>,
        IHandleMessages<Commands.Destroy>
    {
        private readonly IUnitOfWork _uow;
        private readonly IBus _bus;

        public Handler(IUnitOfWork uow, IBus bus)
        {
            _uow = uow;
            _bus = bus;
        }

        public void Handle(Commands.Create command)
        {
            var paymentMethod = _uow.R<PaymentMethod>().Get(command.PaymentMethodId);
            var detail = paymentMethod.E<Detail>().New(command.DetailId);
            detail.Create(command.Name, command.Hint);
        }

        public void Handle(Commands.Destroy command)
        {
            var paymentMethod = _uow.R<PaymentMethod>().Get(command.PaymentMethodId);
            var detail = paymentMethod.E<Detail>().Get(command.DetailId);
            detail.Destroy();
        }
    }
}