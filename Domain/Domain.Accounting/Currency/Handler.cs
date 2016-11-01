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
        IHandleMessages<Commands.Activate>,
        IHandleMessages<Commands.AddRate>,
        IHandleMessages<Commands.ChangeAccuracy>,
        IHandleMessages<Commands.ChangeName>,
        IHandleMessages<Commands.ChangeRoundingFactor>,
        IHandleMessages<Commands.ChangeSymbol>,
        IHandleMessages<Commands.Create>,
        IHandleMessages<Commands.Deactivate>,
        IHandleMessages<Commands.SymbolBefore>,
        IHandleMessages<Commands.ChangeFormat>,
        IHandleMessages<Commands.ChangeFraction>
    {
        private readonly IUnitOfWork _uow;
        

        public Handler(IUnitOfWork uow)
        {
            _uow = uow;
           
        }

        public async Task Handle(Commands.Create command, IMessageHandlerContext ctx)
        {
            var user = await _uow.For<Authentication.Users.User>().Get(command.CurrentUserId);
            var currency = await _uow.For<Currency>().New(command.CurrencyId);
            currency.Create(command.Code, command.Name, command.Symbol, command.SymbolBefore, command.RoundingFactor, command.ComputationalAccuracy, command.Format, command.Fraction);
        }

        public async Task Handle(Commands.Activate command, IMessageHandlerContext ctx)
        {
            var currency = await _uow.For<Currency>().Get(command.CurrencyId);
            var user = await _uow.For<Authentication.Users.User>().Get(command.CurrentUserId);
            currency.Activate(user);
        }

        public async Task Handle(Commands.AddRate command, IMessageHandlerContext ctx)
        {
            var currency = await _uow.For<Currency>().Get(command.CurrencyId);
            var destination = await _uow.For<Currency>().Get(command.DestinationCurrencyId);
            currency.AddRate(command.Factor, destination, command.EffectiveTill);
        }

        public async Task Handle(Commands.ChangeFormat command, IMessageHandlerContext ctx)
        {
            var currency = await _uow.For<Currency>().Get(command.CurrencyId);
            currency.ChangeFormat(command.Format);
        }

        public async Task Handle(Commands.ChangeFraction command, IMessageHandlerContext ctx)
        {
            var currency = await _uow.For<Currency>().Get(command.CurrencyId);
            currency.ChangeFraction(command.Fraction);
        }

        public async Task Handle(Commands.ChangeAccuracy command, IMessageHandlerContext ctx)
        {
            var currency = await _uow.For<Currency>().Get(command.CurrencyId);
            currency.ChangeAccuracy(command.ComputationalAccuracy);
        }

        public async Task Handle(Commands.ChangeName command, IMessageHandlerContext ctx)
        {
            var currency = await _uow.For<Currency>().Get(command.CurrencyId);
            currency.ChangeName(command.Name);
        }

        public async Task Handle(Commands.ChangeRoundingFactor command, IMessageHandlerContext ctx)
        {
            var currency = await _uow.For<Currency>().Get(command.CurrencyId);
            currency.ChangeRoundingFactor(command.RoundingFactor);
        }

        public async Task Handle(Commands.ChangeSymbol command, IMessageHandlerContext ctx)
        {
            var currency = await _uow.For<Currency>().Get(command.CurrencyId);
            currency.ChangeSymbol(command.Symbol);
        }

        public async Task Handle(Commands.Deactivate command, IMessageHandlerContext ctx)
        {
            var currency = await _uow.For<Currency>().Get(command.CurrencyId);
            var user = await _uow.For<Authentication.Users.User>().Get(command.CurrentUserId);
            currency.Deactivate(user);
        }

        public async Task Handle(Commands.SymbolBefore command, IMessageHandlerContext ctx)
        {
            var currency = await _uow.For<Currency>().Get(command.CurrencyId);
            currency.ChangeSymbolBefore(command.Before);
        }

        public async Task Handle(Commands.Destroy command, IMessageHandlerContext ctx)
        {
            var currency = await _uow.For<Currency>().Get(command.CurrencyId);
            currency.Destroy();
        }
    }
}