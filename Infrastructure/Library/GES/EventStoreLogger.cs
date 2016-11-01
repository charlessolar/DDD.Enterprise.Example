using EventStore.ClientAPI;
using System;

namespace Demo.Library.GES
{
    public class EventStoreLogger : ILogger
    {
        private static readonly NLog.ILogger Logger = NLog.LogManager.GetLogger("EventStore");

        public void Debug(string format, params object[] args)
        {
            Logger.Info(format, args);
        }

        public void Debug(Exception ex, string format, params object[] args)
        {
            Logger.Info(format + " Exception: " + ex.ToString(), args);
        }

        public void Error(string format, params object[] args)
        {
            Logger.Error(format, args);
        }

        public void Error(Exception ex, string format, params object[] args)
        {
            Logger.Error(format + " Exception: " + ex.ToString(), args);
        }

        public void Info(string format, params object[] args)
        {
            Logger.Info(format, args);
        }

        public void Info(Exception ex, string format, params object[] args)
        {
            Logger.Info(format + " Exception: " + ex.ToString(), args);
        }
    }
}