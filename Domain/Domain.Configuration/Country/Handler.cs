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
        IHandleMessagesAsync<Commands.Create>,
        IHandleMessagesAsync<Commands.UpdateName>,
        IHandleMessagesAsync<Commands.Destroy>
    {
        private readonly IUnitOfWork _uow;
        private readonly IBus _bus;

        public Handler(IUnitOfWork uow, IBus bus)
        {
            _uow = uow;
            _bus = bus;
        }

        public async Task Handle(Commands.Create command, IHandleContext ctx)
        {
            var country = await _uow.For<Country>().New(command.CountryId);
            country.Create(command.Code, command.Name);
        }

        public async Task Handle(Commands.Destroy command, IHandleContext ctx)
        {
            var country = await _uow.For<Country>().Get(command.CountryId);
            country.Destroy();
        }

        public async Task Handle(Commands.UpdateName command, IHandleContext ctx)
        {
            var country = await _uow.For<Country>().Get(command.CountryId);
            country.ChangeName(command.Name);
        }
    }
}