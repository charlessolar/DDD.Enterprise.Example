using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.MessageInterfaces;
using NServiceBus.Transport.RabbitMQ;

namespace Demo.Library.RabbitMq
{
    public class RoutedFeature : NServiceBus.Features.Feature
    {
        protected override void Setup(FeatureConfigurationContext context)
        {
            context.RegisterStartupTask<TopologyRunner>(b =>
            {
                var routingTopology = context.Settings.Get<IRoutingTopology>();
                if (routingTopology is RoutingRoutingTopology)
                    (routingTopology as RoutingRoutingTopology).Mapper = b.Build<IMessageMapper>();
                
                return new TopologyRunner();
            });

            context.Pipeline.Register(
                b => new BindRoutedBehavior(context.Settings.InstanceSpecificQueue(), b.Build<IMessageMapper>()),
                description: "Detects and binds the queue for routed messages"
                );
            context.Pipeline.Register(
                b => new OutgoingRoutingKeyHeader(b.Build<IMessageMapper>()),
                description: "Adds a header to the message if its routable"
                );
        }
    }

    public class TopologyRunner : FeatureStartupTask
    {
        protected override Task OnStart(IMessageSession session)
        {
            return Task.CompletedTask;
        }

        protected override Task OnStop(IMessageSession session)
        {
            return Task.CompletedTask;
        }
    }
}
