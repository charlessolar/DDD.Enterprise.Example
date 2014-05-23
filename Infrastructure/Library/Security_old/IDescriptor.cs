using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security
{
    public interface IDescriptor
    {
        IDescriptor When { get; }

        void AddAction(IAction action);
        IEnumerable<IAction> Actions { get; }

        bool CanAuthorize(object instance);

        AuthorizeDescriptorResult Authorize(object instance);
    }
}