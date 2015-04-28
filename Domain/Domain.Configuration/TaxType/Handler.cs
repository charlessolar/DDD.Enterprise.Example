using Aggregates;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.TaxType
{
    public class Handler :
        IHandleMessages<Commands.ChangeName>,
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

        public void Handle(Commands.ChangeName command)
        {
            var tax = _uow.R<TaxType>().Get(command.TaxTypeId);
            tax.ChangeName(command.Name);
        }

        public void Handle(Commands.Create command)
        {
            var tax = _uow.R<TaxType>().New(command.TaxTypeId);
            tax.Create(command.Name);
        }

        public void Handle(Commands.Destroy command)
        {
            var tax = _uow.R<TaxType>().New(command.TaxTypeId);
            tax.Destroy();
        }
    }
}