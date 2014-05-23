using System;

namespace Demo.Library.Security.Securables
{
    /// <summary>
    /// Represents a <see cref="Securable"/> that applies to a specific namespace
    /// </summary>
    public class NamespaceSecurable : Securable
    {
        private const string NAMESPACE = "InNamespace_{{{0}}}";

        /// <summary>
        /// Initializes a new instance of <see cref="NamespaceSecurable"/>
        /// </summary>
        /// <param name="namespace">Namespace to secure</param>
        public NamespaceSecurable(string @namespace)
            : base(string.Format(NAMESPACE, @namespace))
        {
            Namespace = @namespace;
        }

        /// <summary>
        /// Gets the namespace that is secured
        /// </summary>
        public string Namespace { get; private set; }

#pragma warning disable 1591
        public override bool CanAuthorize(object instance)
        {
            return instance != null && instance.GetType().Namespace.StartsWith(Namespace, StringComparison.InvariantCulture);
        }
#pragma warning restore 1591
    }
}