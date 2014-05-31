using Castle.DynamicProxy;
using Demo.Library.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Extensions
{
    public static class SecurityExtension
    {
        public static String GetSecurityContext(this Type Type)
        {
            return Type.FullName;
        }

        public static String GetSecurityContext(this PropertyInfo Property)
        {
            return String.Format("{0}/{2}", Property.DeclaringType.FullName, Property.Name);
        }

        public static String GetSecurityContext(this TypeInfo Type)
        {
            return Type.FullName;
        }

        public static String GetSecurityContext(this MethodInfo Method)
        {
            if (Method.IsSpecialName && (Method.Name.StartsWith("get_") || Method.Name.StartsWith("set_")))
            {
                // Is property
                return String.Format("{0}/{1}", Method.DeclaringType.FullName, Method.Name.Substring(4));
            }
            return String.Format("{0}/{1}", Method.DeclaringType.FullName, Method.Name);
        }
    }
}