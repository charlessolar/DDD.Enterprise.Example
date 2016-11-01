using NLog;
using NLog.LayoutRenderers;
using System;
using System.Configuration;
using System.Text;

namespace Demo.Library.Logging
{
    [LayoutRenderer("logsdir")]
    public class LogsDir : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            var logsdir = ConfigurationManager.AppSettings["logsdir"];
            if (logsdir.StartsWith("."))
                logsdir = AppDomain.CurrentDomain.BaseDirectory + "/" + logsdir;
            builder.Append(logsdir);
        }
    }
}
