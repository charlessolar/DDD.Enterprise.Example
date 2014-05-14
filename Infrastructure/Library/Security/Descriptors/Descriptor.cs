using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security.Descriptors
{
    public class Descriptor : IDescriptor
    {
        private List<IAction> _actions = new List<IAction>();


#pragma warning disable 1591 // Xml Comments

        public IDescriptor When { get { return this; } }

        public void AddAction(IAction action)
        {
            _actions.Add(action);
        }

        public IEnumerable<IAction> Actions { get { return _actions; } }

        public bool CanAuthorize(object instance)
        {
            return _actions.Any(a => a.CanAuthorize(instance));
        }

        public AuthorizeDescriptorResult Authorize(object instance)
        {
            var result = new AuthorizeDescriptorResult();
            foreach (var a in Actions.Where(a => a.CanAuthorize(instance)))
            {
                result.ProcessAuthorizeActionResult(a.Authorize(instance));
            }
            return result;
        }
#pragma warning restore 1591 // Xml Comments
    }
}