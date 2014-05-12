using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security
{
    /// <summary>
    /// Determines if a rule should be executed
    /// </summary>
    public interface ISelector
    {
        bool CanExecute(IRule rule);
    }
}