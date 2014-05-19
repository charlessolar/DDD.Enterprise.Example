using Demo.Library.Queries;
using Demo.Library.Security.Securables;
using Demo.Library.Security.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security
{
    public static class SecurableExtensions
    {
        public static TypeSecurable OfType<T>(this QueryTarget target, Action<TypeSecurable> continueWith = null) where T : BasicQuery
        {
            var securable = new TypeSecurable(typeof(T));
            target.AddSecurable(securable);
            if (continueWith != null)
                continueWith.Invoke(securable);
            return securable;
        }

        public static TypeSecurable OnType<T>(this FunctionTarget target, Action<TypeSecurable> continueWith = null) where T : class
        {
            var securable = new TypeSecurable(typeof(T));
            target.AddSecurable(securable);
            if (continueWith != null)
                continueWith.Invoke(securable);
            return securable;
        }

        public static NamespaceSecurable InNamespace(this FunctionTarget target, String @namespace, Action<NamespaceSecurable> continueWith = null)
        {
            var securable = new NamespaceSecurable(@namespace);
            target.AddSecurable(securable);
            if (continueWith != null)
                continueWith.Invoke(securable);
            return securable;
        }

        public static PropertySecurable InType<T>(this PropertyTarget target, String property, Action<PropertySecurable> continueWith = null) where T : class
        {
            var securable = new PropertySecurable(typeof(T), property);
            target.AddSecurable(securable);
            if (continueWith != null)
                continueWith.Invoke(securable);
            return securable;
        }
    }
}