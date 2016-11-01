using ServiceStack;

namespace Demo.Presentation.ServiceStack.Utils.Services
{
    [Api("Utilities")]
    [Route("/util/subscriptions/{SubscriptionId}", "DELETE")]
    public class DropSubscription  : Infrastructure.Commands.ServiceCommand
    {
        public new string SubscriptionId { get; set; }
    }
}
