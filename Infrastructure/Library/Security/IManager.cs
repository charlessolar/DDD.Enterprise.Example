using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security
{
    public interface IManager
    {
        /// <summary>
        /// Authorize the specific action
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        AuthorizationResult Authorize<TAction>(object instance) where TAction : IAction;
    }
}