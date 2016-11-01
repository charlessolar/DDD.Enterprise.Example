using Demo.Library.SSE;
using ServiceStack;

namespace Demo.Presentation.ServiceStack.Infrastructure.Commands
{
    public class ServiceCommand  : IReturnVoid
    {
        public string SubscriptionId { get; set; }

        public int? SubscriptionTime { get; set; }
        public ChangeType? SubscriptionType { get; set; }
    }
}
