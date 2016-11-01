using System;
using System.Linq;

namespace Demo.Library.Algorithms.Strings
{
    public static partial class ComparisonMetrics
    {
        public static double OverlapCoefficient(this string source, string target)
        {
            return (Convert.ToDouble(source.Intersect(target).Count())) / Convert.ToDouble(Math.Min(source.Length, target.Length));
        }
    }
}
