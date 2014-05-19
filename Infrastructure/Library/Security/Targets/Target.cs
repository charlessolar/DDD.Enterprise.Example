using System.Collections.Generic;
using System.Linq;

namespace Demo.Library.Security.Targets
{
    /// <summary>
    /// Represents a base class for any <see cref="ITarget">security targets</see>
    /// </summary>
    public class Target : ITarget
    {
        private readonly List<ISecurable> _securables = new List<ISecurable>();

        /// <summary>
        /// Instantiats an instance of <see cref="SecurityTarget"/>
        /// </summary>
        /// <param name="description">A description for this <see cref="SecurityTarget"/></param>
        public Target(string description)
        {
            Description = description ?? string.Empty;
        }

#pragma warning disable 1591
        public virtual void AddSecurable(ISecurable securityObject)
        {
            _securables.Add(securityObject);
        }

        public virtual IEnumerable<ISecurable> Securables { get { return _securables; } }

        public virtual bool CanAuthorize(object instance)
        {
            return _securables.Any(s => s.CanAuthorize(instance));
        }

        public virtual AuthorizeTargetResult Authorize(object instance)
        {
            var result = new AuthorizeTargetResult(this);
            foreach (var securable in Securables)
            {
                result.ProcessAuthorizeSecurableResult(securable.Authorize(instance));
            }
            return result;
        }

        public string Description { get; private set; }

#pragma warning restore 1591
    }
}