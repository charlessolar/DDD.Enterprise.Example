using Aggregates;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.Country
{
    public class Handler :
        IHandleMessages<Commands.Create>,
        IHandleMessages<Commands.UpdateName>,
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
            var country = _uow.R<Country>().New(command.CountryId);
            country.Create(command.Code, command.Name);
        }

        public void Handle(Commands.Destroy command)
        {
            var country = _uow.R<Country>().Get(command.CountryId);
            country.Destroy();
        }

        public void Handle(Commands.UpdateName command)
        {
            var country = _uow.R<Country>().Get(command.CountryId);
            country.ChangeName(command.Name);
        }
    }
}