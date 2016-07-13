using EventStore.ClientAPI;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo.Library.GES
{
    public class EventStoreLogger : ILogger
    {
        private static NLog.ILogger _logger = NLog.LogManager.GetLogger("EventStore");

        public void Debug(string format, params object[] args)
        {
            _logger.Info(format, args);
        }

        public void Debug(Exception ex, string format, params object[] args)
        {
            _logger.Info(format + " Exception: " + ex.ToString(), args);
        }

        public void Error(string format, params object[] args)
        {
            _logger.Error(format, args);
        }

        public void Error(Exception ex, string format, params object[] args)
        {
            _logger.Error(format + " Exception: " + ex.ToString(), args);
        }

        public void Info(string format, params object[] args)
        {
            _logger.Info(format, args);
        }

        public void Info(Exception ex, string format, params object[] args)
        {
            _logger.Info(format + " Exception: " + ex.ToString(), args);
        }
    }
}