using Castle.DynamicProxy;
using StructureMap;
using System;

namespace Demo.Library.Security.Securables
{
    /// <summary>
    /// Represents a <see cref="Securable"/> that applies to a specific <see cref="System.Type"/>
    /// </summary>
    public class TypeSecurable : Securable
    {
        private const string TYPE = "OfType_{{{0}}}";

        /// <summary>
        /// Initializes an instance of <see cref="TypeSecurable"/>
        /// </summary>
        /// <param name="type"><see cref="System.Type"/> to secure</param>
        public TypeSecurable(Type type)
            : base(string.Format(TYPE, type.FullName))
        {
            Type = type;
        }


        /// <summary>
        /// Gets the type that is secured
        /// </summary>
        public Type Type { get; private set; }

#pragma warning disable 1591
        public override bool CanAuthorize(object instance)
        {
            return instance != null && Type == instance.GetType();
        }
#pragma warning restore 1591
    }
}