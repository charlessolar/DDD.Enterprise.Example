using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security
{
    public interface IDescriptor
    {
        IBuilder When { get; }

        void AddAction(IAction action);
        IEnumerable<IAction> Actions { get; }

        bool CanAuthorize<TAction>(object instance) where TAction : IAction;

        AuthorizeDescriptorResult Authorize(object instance);
    }
}