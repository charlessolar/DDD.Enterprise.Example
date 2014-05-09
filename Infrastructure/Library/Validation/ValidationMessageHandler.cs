using FluentValidation;
using NServiceBus;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Validation
{
    public class ValidationMessageHandler : IHandleMessages<IMessage>
    {
        private readonly IBus _bus;
        private readonly IContainer _container;

        public ValidationMessageHandler(IBus bus, IContainer container)
        {
            _bus = bus;
            _container = container;
        }

        public void Handle(IMessage message)
        {
            var messageType = message.GetType();

            var interfaces = messageType.GetInterfaces()
                .Where(messageInterface => typeof(IMessage).IsAssignableFrom(messageInterface))
                .Where(messageInterface => messageInterface != typeof(IMessage))
                .Select(messageInterface => typeof(IValidator<>).MakeGenericType(new[] { messageInterface }));
            var validationErrors = interfaces.SelectMany(validatorInterface => _container.GetAllInstances(validatorInterface)
                    .Cast<IValidator>())
                .Select(validator => validator.Validate(message))
                .ToList();

            if (validationErrors.Count == 0)
            {
                return;
            }

            _bus.DoNotContinueDispatchingCurrentMessageToHandlers();
        }
    }
}