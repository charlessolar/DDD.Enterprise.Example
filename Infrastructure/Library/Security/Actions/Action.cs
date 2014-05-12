using System.Collections.Generic;
using System.Linq;

namespace Demo.Library.Security.Actions
{
    /// <summary>
    /// Represents a base for any <see cref="IAction"/>
    /// </summary>
    public class Action : IAction
    {
        private readonly List<ITarget> _targets = new List<ITarget>();

        public Action(string description)
        {
            Description = description ?? string.Empty;
        }

#pragma warning disable 1591 // Xml Comments
        public void AddTarget(ITarget securityTarget)
        {
            _targets.Add(securityTarget);
        }

        public bool CanAuthorize(object instance)
        {
            return _targets.Any(s => s.CanAuthorize(instance));
        }

        public AuthorizeActionResult Authorize(object instance)
        {
            var result = new AuthorizeActionResult(this);
            foreach (var target in Targets.Where(t => t.CanAuthorize(instance)))
            {
                result.ProcessAuthorizeTargetResult(target.Authorize(instance));
            }
            return result;
        }

        public string Description { get; private set; }

        public IEnumerable<ITarget> Targets { get { return _targets.AsEnumerable(); } }
#pragma warning restore 1591 // Xml Comments
    }
}