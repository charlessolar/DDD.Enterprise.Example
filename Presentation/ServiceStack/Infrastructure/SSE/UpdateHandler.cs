using NServiceBus;
using Demo.Library.Updates;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NServiceBus.Logging;

namespace Demo.Presentation.ServiceStack.Infrastructure.SSE
{
    public class UpdateHandler : IHandleMessages<IUpdate>
    {
        private readonly ISubscriptionManager _manager;

        public UpdateHandler(ISubscriptionManager manager)
        {
            _manager = manager;
        }

        public Task Handle(IUpdate e, IMessageHandlerContext ctx)
        {
            if(e.Payload==null)
                LogManager.GetLogger("Update").Warn($"Received null update: {JsonConvert.SerializeObject((e))}");
            return _manager.Publish(e.Payload, e.ChangeType);
        }
    }
}
