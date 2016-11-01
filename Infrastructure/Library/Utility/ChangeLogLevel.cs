using NLog;
using Demo.Library.Command;

namespace Demo.Library.Utility
{
    public interface IChangeLogLevel : IDemoEvent
    {
        LogLevel Level { get; set; }
    }
}
