using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Demo.Library.Extensions
{
    public static class ObjectExtensions
    {
        public static IDictionary<string, object> ToDictionary(this object source)
        {
            return source.ToDictionary<object>();
        }

        public static IDictionary<string, T> ToDictionary<T>(this object source)
        {
            if (source == null)
                ThrowExceptionWhenSourceArgumentIsNull();

            var dictionary = new Dictionary<string, T>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
                AddPropertyToDictionary<T>(property, source, dictionary);
            return dictionary;
        }

        private static void AddPropertyToDictionary<T>(PropertyDescriptor property, object source, Dictionary<string, T> dictionary)
        {
            object value = property.GetValue(source);
            if (IsOfType<T>(value))
                dictionary.Add(property.Name, (T)value);
        }

        private static bool IsOfType<T>(object value)
        {
            return value is T;
        }

        private static void ThrowExceptionWhenSourceArgumentIsNull()
        {
            throw new ArgumentNullException("source", "Unable to convert object to a dictionary. The source object is null.");
        }

        public static bool IsDatetime(this object expression)
        {
            DateTime? temp;
            return expression.ToDatetime(out temp);
        }
        public static bool ToDatetime(this object expression, out DateTime? outer)
        {
            outer = null;
            if (expression == null)
                return false;

            DateTime parsed;
            if (DateTime.TryParseExact(Convert.ToString(expression, CultureInfo.InvariantCulture), DateTimeFormatInfo.CurrentInfo.GetAllDateTimePatterns('d'), CultureInfo.CurrentCulture, DateTimeStyles.None, out parsed))
                outer = parsed;
            return outer.HasValue;
        }
        public static bool IsBoolean(this object expression)
        {
            bool? temp;
            return expression.ToBoolean(out temp);
        }
        public static bool ToBoolean(this object expression, out bool? outer)
        {
            outer = null;
            if (expression == null)
                return false;

            bool parsed;
            if (bool.TryParse(Convert.ToString(expression, CultureInfo.InvariantCulture), out parsed))
                outer = parsed;
            return outer.HasValue;
        }
        public static bool IsGuid(this object expression)
        {
            Guid? temp;
            return expression.ToGuid(out temp);
        }
        public static bool ToGuid(this object expression, out Guid? outer)
        {
            outer = null;
            if (expression == null)
                return false;

            Guid parsed;
            if (Guid.TryParse(Convert.ToString(expression, CultureInfo.InvariantCulture), out parsed))
                outer = parsed;
            return outer.HasValue;
        }
        public static bool IsNumeric(this object expression)
        {
            if (expression == null)
                return false;

            return Regex.IsMatch(Convert.ToString(expression, CultureInfo.InvariantCulture), @"^(\d+|\d{1,3}(,\d{3})*)(\.\d+)?$", RegexOptions.Compiled);
        }
        public static bool IsInteger(this object expression)
        {
            long? temp;
            return expression.ToInteger(out temp);
        }
        public static bool ToInteger(this object expression, out long? outer)
        {
            outer = null;
            if (expression == null)
                return false;

            long parsed;
            if(long.TryParse(Convert.ToString(expression, CultureInfo.InvariantCulture), 
                NumberStyles.Integer | NumberStyles.AllowParentheses | NumberStyles.AllowThousands | NumberStyles.AllowTrailingSign, 
                CultureInfo.InvariantCulture, out parsed))
                outer = parsed;
            return outer.HasValue;
        }
        public static bool IsDouble(this object expression)
        {
            double? temp;
            return expression.ToDouble(out temp);
        }
        public static bool ToDouble(this object expression, out double? outer)
        {
            outer = null;
            if (expression == null)
                return false;

            double parsed;
            if (double.TryParse(Convert.ToString(expression, CultureInfo.InvariantCulture),
                NumberStyles.AllowThousands | NumberStyles.AllowTrailingSign | NumberStyles.Currency | NumberStyles.Float, CultureInfo.InvariantCulture, out parsed))
                outer = parsed;
            return outer.HasValue;
        }
    }
}
