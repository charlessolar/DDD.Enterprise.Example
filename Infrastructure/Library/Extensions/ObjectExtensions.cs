using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        public static Boolean IsDatetime(this object expression)
        {
            DateTime? temp;
            return expression.ToDatetime(out temp);
        }
        public static Boolean ToDatetime(this object expression, out DateTime? outer)
        {
            outer = null;
            if (expression == null)
                return false;

            DateTime parsed;
            if (DateTime.TryParseExact(Convert.ToString(expression, CultureInfo.InvariantCulture), DateTimeFormatInfo.CurrentInfo.GetAllDateTimePatterns('d'), CultureInfo.CurrentCulture, DateTimeStyles.None, out parsed))
                outer = parsed;
            return outer.HasValue;
        }
        public static Boolean IsBoolean(this object expression)
        {
            Boolean? temp;
            return expression.ToBoolean(out temp);
        }
        public static Boolean ToBoolean(this object expression, out Boolean? outer)
        {
            outer = null;
            if (expression == null)
                return false;

            Boolean parsed;
            if (Boolean.TryParse(Convert.ToString(expression, CultureInfo.InvariantCulture), out parsed))
                outer = parsed;
            return outer.HasValue;
        }
        public static Boolean IsGuid(this object expression)
        {
            Guid? temp;
            return expression.ToGuid(out temp);
        }
        public static Boolean ToGuid(this object expression, out Guid? outer)
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
        public static Boolean IsInteger(this object expression)
        {
            Int64? temp;
            return expression.ToInteger(out temp);
        }
        public static Boolean ToInteger(this object expression, out Int64? outer)
        {
            outer = null;
            if (expression == null)
                return false;

            Int64 parsed;
            if( Int64.TryParse(Convert.ToString(expression, CultureInfo.InvariantCulture), 
                NumberStyles.Integer | NumberStyles.AllowParentheses | NumberStyles.AllowThousands | NumberStyles.AllowTrailingSign, 
                CultureInfo.InvariantCulture, out parsed))
                outer = parsed;
            return outer.HasValue;
        }
        public static Boolean IsDouble(this object expression)
        {
            Double? temp;
            return expression.ToDouble(out temp);
        }
        public static Boolean ToDouble(this object expression, out Double? outer)
        {
            outer = null;
            if (expression == null)
                return false;

            Double parsed;
            if (Double.TryParse(Convert.ToString(expression, CultureInfo.InvariantCulture),
                NumberStyles.AllowThousands | NumberStyles.AllowTrailingSign | NumberStyles.Currency | NumberStyles.Float, CultureInfo.InvariantCulture, out parsed))
                outer = parsed;
            return outer.HasValue;
        }
    }
}
