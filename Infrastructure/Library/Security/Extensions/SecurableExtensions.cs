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
        public static TypeSecurable OfType<T>(this QueryTarget target) where T : BasicQuery
        {
            var securable = new TypeSecurable(typeof(T));
            target.AddSecurable(securable);
            return securable;
        }

        public static TypeSecurable OfType<T>(this QueryTarget target, Action<TypeSecurable> continueWith) where T : BasicQuery
        {
            var securable = new TypeSecurable(typeof(T));
            target.AddSecurable(securable);
            continueWith.Invoke(securable);
            return securable;
        }

        public static TypeSecurable OnType<T>(this FunctionTarget target) where T : class
        {
            var securable = new TypeSecurable(typeof(T));
            target.AddSecurable(securable);
            return securable;
        }

        public static NamespaceSecurable InNamespace(this FunctionTarget target, String @namespace)
        {
            var securable = new NamespaceSecurable(@namespace);
            target.AddSecurable(securable);
            return securable;
        }

        public static PropertySecurable InType<T>(this PropertyTarget target, String property)
        {
            var securable = new PropertySecurable(typeof(T), property);
            target.AddSecurable(securable);
            return securable;
        }
    }
}