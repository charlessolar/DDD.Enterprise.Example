using NServiceBus;

namespace Demo.Library.Command
{
    public interface IDemoEvent : IEvent
    {
        string CurrentUserId { get; set; }
        long Stamp { get; set; }
    }
}
