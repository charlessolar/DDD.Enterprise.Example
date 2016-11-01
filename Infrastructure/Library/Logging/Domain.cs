using NLog;
using NLog.LayoutRenderers;
using System.Text;

namespace Demo.Library.Logging
{
    [LayoutRenderer("Instance")]
    public class Instance : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            builder.Append(Aggregates.Defaults.Instance);
        }
    }
}
