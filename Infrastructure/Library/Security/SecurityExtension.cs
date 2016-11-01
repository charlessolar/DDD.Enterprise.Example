using System;
using System.Reflection;

namespace Demo.Library.Extensions
{
    public static class SecurityExtension
    {
        public static string GetSecurityContext(this Type type)
        {
            return type.FullName;
        }

        public static string GetSecurityContext(this PropertyInfo property)
        {
            return string.Format("{0}/{2}", property.DeclaringType.FullName, property.Name);
        }

        public static string GetSecurityContext(this TypeInfo type)
        {
            return type.FullName;
        }

        public static string GetSecurityContext(this MethodInfo method)
        {
            if (method.IsSpecialName && (method.Name.StartsWith("get_") || method.Name.StartsWith("set_")))
            {
                // Is property
                return $"{method.DeclaringType.FullName}/{method.Name.Substring(4)}";
            }
            return $"{method.DeclaringType.FullName}/{method.Name}";
        }
    }
}