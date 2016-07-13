using NLog;
using NLog.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
