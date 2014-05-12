using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security
{
    public enum CascaseMode
    {
        /// <summary>
        /// When a rule fails, execution continues to the next rule.
        /// </summary>
        Continue,

        /// <summary>
        /// When a rule fails, checking is stopped and all other rules in the chain will not be executed.
        /// </summary>
        StopOnFirstFailure
    }
}