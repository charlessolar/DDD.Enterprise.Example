using EventStore.Common.Log;
using System;

namespace Demo.Domain.Infrastructure.GES
{
    public class EmbeddedLogger : ILogger
    {
        private readonly NLog.ILogger _logger;
        public EmbeddedLogger(string name)
        {
            _logger = NLog.LogManager.GetLogger(name);
        }
        public void Debug(string format, params object[] args)
        {
            try
            {
                format = string.Format(format, args);
            }
            catch (FormatException)
            { }
            _logger.Debug(format);
        }

        public void DebugException(Exception ex, string format, params object[] args)
        {
            try
            {
                format = string.Format(format + " Exception: " + ex.ToString(), args);
            }
            catch (FormatException)
            { }
            _logger.Debug(format);
        }

        public void Error(string format, params object[] args)
        {
            try
            {
                format = string.Format(format, args);
            }
            catch (FormatException)
            { }
            _logger.Error(format);
        }

        public void ErrorException(Exception ex, string format, params object[] args)
        {
            try
            {
                format = string.Format(format + " Exception: " + ex.ToString(), args);
            }
            catch (FormatException)
            { }
            _logger.Error(format);
        }

        public void Fatal(string format, params object[] args)
        {
            try
            {
                format = string.Format(format, args);
            }
            catch (FormatException)
            { }
            _logger.Fatal(format);
        }

        public void FatalException(Exception ex, string format, params object[] args)
        {
            try
            {
                format = string.Format(format + " Exception: " + ex.ToString(), args);
            }
            catch (FormatException)
            { }
            _logger.Fatal(format);
        }

        public void Flush(TimeSpan? maxTimeToWait = default(TimeSpan?))
        {
        }

        public void Info(string format, params object[] args)
        {
            try
            {
                format = string.Format(format, args);
            }
            catch (FormatException)
            { }
            _logger.Info(format);
        }

        public void InfoException(Exception ex, string format, params object[] args)
        {
            try
            {
                format = string.Format(format + " Exception: " + ex.ToString(), args);
            }
            catch (FormatException)
            { }
            _logger.Info(format);
        }

        public void Trace(string format, params object[] args)
        {
            try
            {
                format = string.Format(format, args);
            }
            catch (FormatException)
            { }
            _logger.Debug(format);
        }

        public void TraceException(Exception ex, string format, params object[] args)
        {
            try
            {
                format = string.Format(format + " Exception: " + ex.ToString(), args);
            }
            catch (FormatException)
            { }
            _logger.Debug(format);
        }

        public void Warn(string format, params object[] args)
        {
            try
            {
                format = string.Format(format, args);
            }
            catch (FormatException)
            { }
            _logger.Warn(format);
        }
    }
}
