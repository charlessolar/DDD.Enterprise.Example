using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security.Securables
{
    public class PropertySecurable : Securable
    {
        private const string PROPERTY = "Property_{{{0}}}";

        /// <summary>
        /// Initializes a new instance of <see cref="NamespaceSecurable"/>
        /// </summary>
        /// <param name="namespace">Namespace to secure</param>
        public PropertySecurable(Type type, string property)
            : base(string.Format(PROPERTY, property))
        {
            Type = type;
            Property = property;
        }

        /// <summary>
        /// Gets property name that is secured
        /// </summary>
        public string Property { get; private set; }
        /// <summary>
        /// The type that holds the secure property
        /// </summary>
        public Type Type { get; private set; }

#pragma warning disable 1591
        public override bool CanAuthorize(object instance)
        {
            return instance != null && instance.GetType() == Type;
        }
#pragma warning restore 1591
    }
}