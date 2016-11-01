using NServiceBus;

namespace Demo.Library.Command
{
    public class DemoCommand : ICommand
    {
        /// <summary>
        /// Should be auto set don't modify
        /// </summary>
        public long Stamp { get; set; }

        /// <summary>
        /// Should be auto set don't modify
        /// </summary>
        public string CurrentUserId { get; set; }
    }
    
}