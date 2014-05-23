using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security.Actions
{
    /// <summary>
    /// A security action for receiving messages
    /// </summary>
    public class ReceivingAction : Action
    {
        public ReceivingAction()
            : base("RECEIVING")
        {
        }
    }
}