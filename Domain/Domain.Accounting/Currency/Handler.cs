using Aggregates;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Currency
{
    public class Handler :
        IHandleMessagesAsync<Commands.Activate>,
        IHandleMessagesAsync<Commands.AddRate>,
        IHandleMessagesAsync<Commands.ChangeAccuracy>,
        IHandleMessagesAsync<Commands.ChangeName>,
        IHandleMessagesAsync<Commands.ChangeRoundingFactor>,
        IHandleMessagesAsync<Commands.ChangeSymbol>,
        IHandleMessagesAsync<Commands.Create>,
        IHandleMessagesAsync<Commands.Deactivate>,
        IHandleMessagesAsync<Commands.SymbolBefore>,
        IHandleMessagesAsync<Commands.ChangeFormat>,
        IHandleMessagesAsync<Commands.ChangeFraction>
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
            var user = await _uow.For<Authentication.Users.User>().Get(command.UserId);
            var currency = await _uow.For<Currency>().New(command.CurrencyId);
            currency.Create(command.Code, command.Name, command.Symbol, command.SymbolBefore, command.RoundingFactor, command.ComputationalAccuracy, command.Format, command.Fraction);
        }

        public async Task Handle(Commands.Activate command, IHandleContext ctx)
        {
            var currency = await _uow.For<Currency>().Get(command.CurrencyId);
            var user = await _uow.For<Authentication.Users.User>().Get(command.UserId);
            currency.Activate(user);
        }

        public async Task Handle(Commands.AddRate command, IHandleContext ctx)
        {
            var currency = await _uow.For<Currency>().Get(command.CurrencyId);
            var destination = await _uow.For<Currency>().Get(command.DestinationCurrencyId);
            currency.AddRate(command.Factor, destination, command.EffectiveTill);
        }

        public async Task Handle(Commands.ChangeFormat command, IHandleContext ctx)
        {
            var currency = await _uow.For<Currency>().Get(command.CurrencyId);
            currency.ChangeFormat(command.Format);
        }

        public async Task Handle(Commands.ChangeFraction command, IHandleContext ctx)
        {
            var currency = await _uow.For<Currency>().Get(command.CurrencyId);
            currency.ChangeFraction(command.Fraction);
        }

        public async Task Handle(Commands.ChangeAccuracy command, IHandleContext ctx)
        {
            var currency = await _uow.For<Currency>().Get(command.CurrencyId);
            currency.ChangeAccuracy(command.ComputationalAccuracy);
        }

        public async Task Handle(Commands.ChangeName command, IHandleContext ctx)
        {
            var currency = await _uow.For<Currency>().Get(command.CurrencyId);
            currency.ChangeName(command.Name);
        }

        public async Task Handle(Commands.ChangeRoundingFactor command, IHandleContext ctx)
        {
            var currency = await _uow.For<Currency>().Get(command.CurrencyId);
            currency.ChangeRoundingFactor(command.RoundingFactor);
        }

        public async Task Handle(Commands.ChangeSymbol command, IHandleContext ctx)
        {
            var currency = await _uow.For<Currency>().Get(command.CurrencyId);
            currency.ChangeSymbol(command.Symbol);
        }

        public async Task Handle(Commands.Deactivate command, IHandleContext ctx)
        {
            var currency = await _uow.For<Currency>().Get(command.CurrencyId);
            var user = await _uow.For<Authentication.Users.User>().Get(command.UserId);
            currency.Deactivate(user);
        }

        public async Task Handle(Commands.SymbolBefore command, IHandleContext ctx)
        {
            var currency = await _uow.For<Currency>().Get(command.CurrencyId);
            currency.ChangeSymbolBefore(command.Before);
        }

        public async Task Handle(Commands.Destroy command, IHandleContext ctx)
        {
            var currency = await _uow.For<Currency>().Get(command.CurrencyId);
            currency.Destroy();
        }
    }
}