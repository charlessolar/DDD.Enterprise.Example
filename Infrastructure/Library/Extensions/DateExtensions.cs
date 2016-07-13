using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Extensions
{
    public static class DateExtensions
    {
        public static String ToISO8601(this DateTime date)
        {
            return date.ToString("o", DateTimeFormatInfo.InvariantInfo);
        }
        public static String ToSQL(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss", DateTimeFormatInfo.InvariantInfo);
        }

        public static Int64 ToUnix(this DateTime date)
        {
            var timeSpan = (date - new DateTime(1970, 1, 1, 0, 0, 0));
            return (Int64)timeSpan.TotalSeconds;
        }
        public static DateTime FromUnix(this Int64 unix)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            return epoch.AddSeconds(unix);
        }
    }
}