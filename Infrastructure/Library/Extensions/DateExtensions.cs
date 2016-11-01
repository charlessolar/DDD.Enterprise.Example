using System;
using System.Globalization;

namespace Demo.Library.Extensions
{
    public static class DateExtensions
    {
        public static string ToIso8601(this DateTime date)
        {
            return date.ToString("o", DateTimeFormatInfo.InvariantInfo);
        }
        public static string ToSql(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss", DateTimeFormatInfo.InvariantInfo);
        }

        public static long ToUnix(this DateTime date)
        {
            var timeSpan = (date - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalSeconds;
        }
        public static DateTime FromUnix(this long unix)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            return epoch.AddSeconds(unix);
        }
    }
}